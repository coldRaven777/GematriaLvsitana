using Castle.Core.Logging;
using GematriaLvistana.Models;
using GematriaLvistana.Repositories;
using GematriaLvistana.Services.Implementation;
using Microsoft.Extensions.Logging;
using Moq;

namespace GematriaLvsitanaTests.Servicos;

public class GematriaServiceTests
{

    private GematriaService _sut;
    private Mock<ILogger<GematriaService>> _loggerMock;
    private Mock<IGematriaRepository> _gematriaRepositoryMock;
    public GematriaServiceTests()
    {
        _loggerMock = new Mock<ILogger<GematriaService>>();
        _gematriaRepositoryMock = new Mock<IGematriaRepository>();
        _sut = new GematriaService(_loggerMock.Object, _gematriaRepositoryMock.Object);
    }

    [Fact]
    public async Task GetWordsByValue_Should_Return_ValidWords()
    {
        // Arrange
        var value = 52;
        var expectedWords = new List<string> { "ana" };
        _gematriaRepositoryMock.Setup(gr => gr.GetWordsByValue(value)).ReturnsAsync(expectedWords);
        // Act
        var result = await _sut.GetWordsByValue(value);
        // Assert
        _gematriaRepositoryMock.Verify(gr => gr.GetWordsByValue(value), Times.Once);
        Assert.Equal(expectedWords, result);
    }

    [Fact]
    public async Task CreateWord_Should_Calculate_Perfectly()
    {
        // Arrange
        var word = "ana";
        var expectedValue = 52;
        //to veryfy if word already exists
        _gematriaRepositoryMock.Setup(gr => gr.GetWord(word)).ReturnsAsync(It.IsAny<WordModel>);
        _gematriaRepositoryMock.Setup(gr => gr.InsertNewWordAsync(It.IsAny<WordModel>()));
   
        // Act
        var result = await _sut.CreateWord(word);
        // Assert
        _gematriaRepositoryMock.Verify(gr => gr.GetWord(word), Times.Once);
        _gematriaRepositoryMock.Verify(gr => gr.InsertNewWordAsync(result), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(word, result.Word);
        Assert.Equal(expectedValue, result.NumericalValue);
        Assert.Equal(word, result.HebrewEquivalent);
    }

    [Fact]
    public async Task CreateWord_Should_NotCreate_ExistingWord()
    {
        // Arrange
        var word = "ana";
        var expectedValue = 52;

        var existingWord = new WordModel
        {
            Word = word,
            HebrewEquivalent = word,
            NumericalValue = expectedValue
        };
        // to verify if word already exists
        _gematriaRepositoryMock.Setup(gr => gr.GetWord(word)).ReturnsAsync(existingWord); // Fix: Replace It.IsAny<int>() with expectedValue
        _gematriaRepositoryMock.Setup(gr => gr.InsertNewWordAsync(It.IsAny<WordModel>()));

        // Act
        var result = await _sut.CreateWord(word);

        // Assert
        _gematriaRepositoryMock.Verify(gr => gr.GetWord(It.IsAny<string>()), Times.Once);
        _gematriaRepositoryMock.Verify(gr => gr.InsertNewWordAsync(result), Times.Never);
        Assert.NotNull(result);
        Assert.Equal(word, result.Word);
        Assert.Equal(expectedValue, result.NumericalValue);
        Assert.Equal(word, result.HebrewEquivalent);
    }
}

