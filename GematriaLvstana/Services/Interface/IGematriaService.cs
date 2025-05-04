using GematriaLvistana.Models;

namespace GematriaLvstana.Services.Interface
{
    public interface IGematriaService
    {
        Task<WordModel> CreateWord(string v);
        Task<List<string>> GetWordsByValue(int value);
    }
}
