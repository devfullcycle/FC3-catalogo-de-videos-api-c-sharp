using System.Text.Json;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FluentAssertions;
using Nest;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Genre;

[Collection(nameof(GenreConsumerTestFixture))]
public class GenreConsumerTest: IDisposable
{
    private readonly GenreConsumerTestFixture _fixture;
    private readonly WireMockServer _mockServer = WireMockServer.Start(61001);

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
        var apiResponseBody = JsonSerializer.Serialize(genre, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        _mockServer.Given(
            Request.Create()
                .WithPath($"genres/{genre.Id}")
                .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(apiResponseBody));
        
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
        var apiResponseBody = JsonSerializer.Serialize(genre, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        _mockServer.Given(
                Request.Create()
                    .WithPath($"genres/{genre.Id}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(apiResponseBody));
        
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

    public void Dispose()
    {
        _mockServer.Dispose();
        _fixture.DeleteAll();
    }
}