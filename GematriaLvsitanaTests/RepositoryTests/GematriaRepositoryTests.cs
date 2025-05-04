using Castle.Core.Logging;
using GematriaLvistana.Data;
using GematriaLvistana.Models;
using GematriaLvistana.Repositories;
using GematriaLvsitanaTests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace GematriaLvsitanaTests.RepositoryTests
{
    public class GematriaRepositoryTests
    {
        private readonly GematriaContext _gematriaContext;
        private GematriaRepository _sut;
        public GematriaRepositoryTests()
        {
            // Configurar o logger mock
            var loggerMock = new Mock<ILogger<GematriaRepository>>();
            // Configurar o DbContextOptions para usar o banco de dados em memória
            var options = new DbContextOptionsBuilder<GematriaContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _gematriaContext = new GematriaContext(options);
            // Adicionar dados de teste ao banco de dados em memória
            _gematriaContext.Database.EnsureDeleted();
            _gematriaContext.Database.EnsureCreated();
            DataHelper.SeedData(_gematriaContext);

            _sut = new GematriaRepository(loggerMock.Object);
        }

        [Fact]
        public async Task Repository_Should_AddNewWord()
        {
            //arrange 
            var Expectedword = new WordModel
            {
                Word = "filipe",
                NumericalValue = 215,
                HebrewEquivalent = "פיליפה"
            };


            //act

            await _sut.InsertNewWordAsync(Expectedword);

            //assert
            var result = await _gematriaContext.Words.FirstOrDefaultAsync(w => w.Word == Expectedword.Word);
        }
    }
}
