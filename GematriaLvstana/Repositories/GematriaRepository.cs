using GematriaLvistana.Models;

namespace GematriaLvistana.Repositories
{
    public class GematriaRepository : IGematriaRepository
    {
        private readonly ILogger<GematriaRepository> _logger;
        public GematriaRepository(ILogger<GematriaRepository> logger)
        {
            _logger = logger;
        }
        public Task<WordModel> GetWord(string word)
        {
            throw new NotImplementedException();
        }
        public Task<List<string>> GetWordsByValue(int value)
        {
            throw new NotImplementedException();
        }
        public Task InsertNewWordAsync(WordModel wordModel)
        {
            throw new NotImplementedException();
        }
    }
    
    
}
