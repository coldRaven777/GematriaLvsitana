using GematriaLvistana.Enums;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using GematriaLvistana.Models;
using GematriaLvistana.Repositories;
using GematriaLvstana.Services.Interface;

namespace GematriaLvistana.Services.Implementation
{
    public class GematriaService : IGematriaService
    {
        private ILogger<GematriaService> _logger;
        private IGematriaRepository _repository;

        public GematriaService(ILogger<GematriaService> logger, IGematriaRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }


            public async Task<WordModel> CreateWord(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                    throw new ArgumentException("Input cannot be null or empty");

                var existingWord = await _repository.GetWord(input);
                    if(existingWord != null)
                    {
                        _logger.LogWarning("Word already exists in the database.");
                        return existingWord;
                    }
            string normalized = Normalize(input);
                string transformed = ApplyPhoneticRules(normalized);
                int value = CalculateGematria(transformed);

                var result = new WordModel
                {
                    Word = input,
                    HebrewEquivalent = transformed,
                    NumericalValue = value
                };
            await _repository.InsertNewWordAsync(result);
            return result;
            }



            public Task<List<string>> GetWordsByValue(int value)
            {
                try
                {
                    var words = _repository.GetWordsByValue(value);
                    return words;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while getting words by value.");
                    throw;
                }
            }

        private string Normalize(string input)
        {
            // Remove acentos e normaliza maiúsculas
            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().ToLowerInvariant();
        }

        private string ApplyPhoneticRules(string input)
        {
            string word = input.ToLowerInvariant();

            // Regras mais específicas primeiro
            word = Regex.Replace(word, "ch", "x");
            word = Regex.Replace(word, "lh", "l");
            word = Regex.Replace(word, "nh", "n");
            word = Regex.Replace(word, "ph", "f");
            word = Regex.Replace(word, "qu", "k");
            word = Regex.Replace(word, "gu", "g");

            // c -> s (antes de e, i) ou k (caso contrário)
            word = Regex.Replace(word, "c(?=[ei])", "s");
            word = Regex.Replace(word, "c", "k");

            // ç -> s
            word = word.Replace("ç", "s");

            // g -> j (antes de e, i), g (caso contrário)
            word = Regex.Replace(word, "g(?=[ei])", "j");

            // x → ks (pode ser ajustado para 'sh' se preferir outra abordagem)
            word = word.Replace("x", "ks");

            return word;
        }


        private int CalculateGematria(string transformed)
        {
            int total = 0;
            foreach (char c in transformed.ToUpperInvariant())
            {
                if (Enum.TryParse<WordCorrespondenceEnum>(c.ToString(), out var val))
                {
                    total += (int)val;
                }
            }
            return total;
        }

    }
}

