//This is the in memory database context for the tests. It is used to create a database in memory for testing purposes.
using GematriaLvistana.Data;
using GematriaLvistana.Models;
using Microsoft.EntityFrameworkCore;

namespace GematriaLvsitanaTests.Data
{

    public static class DataHelper
    {
        public static void SeedData(GematriaContext context)
        {
            // Adicionar dados de teste ao banco de dados em memória
            context.Words.AddRange(
                new WordModel { Word = "ana", NumericalValue = 52, HebrewEquivalent = "אנה" },
                new WordModel { Word = "exemplo", NumericalValue = 164, HebrewEquivalent = "אזמפלו" }
            );
            context.SaveChanges();
        }
    }

}
