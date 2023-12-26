using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Genre;

[Collection(nameof(GenreConsumerTestFixture))]
public class GenreConsumerTest: IDisposable
{
    private readonly GenreConsumerTestFixture _fixture;

    public GenreConsumerTest(GenreConsumerTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(GenreEvent_WhenOperationIsCreate_SavesGenre))]
    [Trait("E2E/Consumers", "Genre")]
    [InlineData("c")]
    [InlineData("r")]
    public async Task GenreEvent_WhenOperationIsCreate_SavesGenre(string operation)
    {
        var message = _fixture.BuildValidMessage(operation);
        var genre = _fixture.GetValidGenre(message.Payload.After.Id);
        /// Configurarmos um mock para um HTTP server, para que sempre que uma requisição for enviada
        /// com esse ID, genre seja retornado
        
        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);
        
        var persisted = await _fixture.ElasticClient
            .GetAsync<GenreModel>(genre.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Should().BeEquivalentTo(genre);
    }
    
    [Fact(DisplayName = nameof(GenreEvent_WhenOperationIsUpdate_SavesGenre))]
    [Trait("E2E/Consumers", "Genre")]
    public async Task GenreEvent_WhenOperationIsUpdate_SavesGenre()
    {
        var examplesList = _fixture.GetGenreModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage("u", example);
        var genre = _fixture.GetValidGenre(message.Payload.After.Id);
        /// Configurarmos um mock para um HTTP server, para que sempre que uma requisição for enviada
        /// com esse ID, genre seja retornado
        
        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);
        
        var persisted = await _fixture.ElasticClient
            .GetAsync<GenreModel>(genre.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Should().BeEquivalentTo(genre);
    }
    
    /*[Fact(DisplayName = nameof(GenreEvent_WhenOperationIsDelete_DeletesGenre))]
    [Trait("E2E/Consumers", "Genre")]
    public async Task GenreEvent_WhenOperationIsDelete_DeletesGenre()
    {
        var examplesList = _fixture.GetGenreModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage("d", example);
        var genre = message.Payload.Before;
        
        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);
        
        var persisted = await _fixture.ElasticClient
            .GetAsync<GenreModel>(genre.Id);
        persisted.Found.Should().BeFalse();
    }*/

    public void Dispose() => _fixture.DeleteAll();
}