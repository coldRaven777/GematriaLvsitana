using GematriaLvistana.Models;

namespace GematriaLvistana.Repositories
{
    public interface IGematriaRepository
    {
        Task<WordModel> GetWord(string word);
        Task<List<string>> GetWordsByValue(int value);
        Task InsertNewWordAsync(WordModel wordModel);
    }
}
