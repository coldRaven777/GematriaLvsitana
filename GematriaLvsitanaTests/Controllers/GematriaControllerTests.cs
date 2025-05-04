using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GematriaLvistana.Models;
using GematriaLvstana.Controllers;
using GematriaLvstana.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GematriaLvsitanaTests.Controllers;

public class GematriaControllerTests
{
    private GematriaController sut;
    private  Mock<IGematriaService> _gematriaServiceMock;
    public GematriaControllerTests()
    {
        _gematriaServiceMock = new Mock<IGematriaService>();
        var mockLogger = new Mock<ILogger<GematriaController>>();

        _gematriaServiceMock.Setup( gs => gs.GetWordsByValue(56)).ReturnsAsync(new List<string> { "ana" });

        sut = new GematriaController(mockLogger.Object, _gematriaServiceMock.Object) ;



    }

    #region GET

    [Fact]
    public async Task Controller_Get_ByNumber_Should_Return_ValidWord()
    {
        // Arrange
        var value = 56;
        var expectedWords = new List<string> { "ana" };
        var expectedResponse = new GematriaResponseModel
        {
            Words = expectedWords,
            Count = expectedWords.Count
        };

        // Act
        var result = await sut.GetWordsByValue(value);

        // Assert
        _gematriaServiceMock.Verify(gs => gs.GetWordsByValue(value), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica se o resultado é do tipo OkObjectResult
        var actualResponse = Assert.IsType<GematriaResponseModel>(okResult.Value); // Verifica o valor retornado
        Assert.Equal(expectedResponse.Count, actualResponse.Count); // Compara a contagem
        Assert.Equal(expectedResponse.Words, actualResponse.Words); // Compara as palavras
    }
    [Fact]
    public async Task Controller_Get_ByNumber_Should_Return_BadRequest_For_InvalidValue()
    {
        // Arrange
        var invalidValue = -1;

        // Act
        var result = await sut.GetWordsByValue(invalidValue);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); // Verifica se o resultado é BadRequest
        Assert.Equal("Value must be greater than zero.", badRequestResult.Value); // Verifica a mensagem de erro
    }

    [Fact]
    public async Task Controller_Get_ByNumber_Should_Return_NotFound_When_NoWordsFound()
    {
        // Arrange
        var value = 156800; // Valor que não retorna palavras
        _gematriaServiceMock.Setup(gs => gs.GetWordsByValue(value)).ReturnsAsync(new List<string>()); // Configura o mock para retornar uma lista vazia

        // Act
        var result = await sut.GetWordsByValue(value);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result); // Verifica se o resultado é NotFound
        Assert.Equal("No words found with the specified value.", notFoundResult.Value); // Verifica a mensagem de erro
    }
    #endregion

    #region registerWord
    [Fact]
    public async Task Controller_CreateWord_Should_CreateNewWordModel()
    {
        // Arrange
        WordModel expectedWord = new WordModel
        {
            Id = 1,
            Word = "ana",
            NumericalValue = 56,
            HebrewEquivalent = "אנה"
        };

        _gematriaServiceMock.Setup(gs => gs.CreateWord("ana")).ReturnsAsync(expectedWord);

        //act 
        var result = await sut.CreateWord("ana");

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica se o resultado é do tipo OkObjectResult
        var actualWord = Assert.IsType<WordModel>(okResult.Value); // Verifica o valor retornado
        Assert.Equal(expectedWord.Id, actualWord.Id); // Compara o Id
        Assert.Equal(expectedWord.Word, actualWord.Word); // Compara a palavra
        Assert.Equal(expectedWord.NumericalValue, actualWord.NumericalValue); // Compara o valor numérico
        Assert.Equal(expectedWord.HebrewEquivalent, actualWord.HebrewEquivalent); // Compara o equivalente hebraico
    }
    #endregion 
}

