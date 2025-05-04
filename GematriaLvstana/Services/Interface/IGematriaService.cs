namespace GematriaLvstana.Services.Interface
{
    public interface IGematriaService
    {
        Task<List<string>> GetWordsByValue(int value);
    }
}
