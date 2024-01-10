using System.Text.Json;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Infra.HttpClients.Configuration;
using FC.Codeflix.Catalog.Infra.HttpClients.Models;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using FluentAssertions;
using Nest;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Genre;

[Collection(nameof(GenreConsumerTestFixture))]
public class GenreConsumerTest : IDisposable
{
    private readonly GenreConsumerTestFixture _fixture;
    private readonly WireMockServer _mockServer = WireMockServer.Start(5555);

    public GenreConsumerTest(GenreConsumerTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ConfigureGetTokenMock(_mockServer);
    }

    [Theory(DisplayName = nameof(GenreEvent_WhenOperationIsCreateOrRead_SavesGenre))]
    [Trait("E2E/Consumers", "Genre")]
    [InlineData("c", nameof(GenrePayloadModel))]
    [InlineData("r", nameof(GenrePayloadModel))]
    [InlineData("c", nameof(GenreCategoryPayloadModel))]
    [InlineData("r", nameof(GenreCategoryPayloadModel))]
    public async Task GenreEvent_WhenOperationIsCreateOrRead_SavesGenre(string operation, string messageType)
    {
        dynamic message = messageType == nameof(GenreCategoryPayloadModel)
            ? _fixture.BuildValidMessage<GenreCategoryPayloadModel>(operation)
            : _fixture.BuildValidMessage<GenrePayloadModel>(operation);
        Domain.Entity.Genre genre = _fixture.GetValidGenre(message.Payload.After.Id);
        var apiResponse = new DataWrapper<Domain.Entity.Genre>(genre);
        var apiResponseBody = JsonSerializer.Serialize(apiResponse, SerializerConfiguration.SnakeCaseSerializerOptions);
        var adminCatalogRequest = Request.Create()
            .WithPath($"/genres/{genre.Id}")
            .WithHeader("Authorization", "Bearer *")
            .UsingGet();
        var adminCatalogResponse = Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(apiResponseBody);
        _mockServer.Given(adminCatalogRequest)
            .RespondWith(adminCatalogResponse);

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await _fixture.ElasticClient
            .GetAsync<GenreModel>(genre.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(genre.Id);
        document.Name.Should().Be(genre.Name);
        document.CreatedAt.Should().Be(genre.CreatedAt);
        document.IsActive.Should().Be(genre.IsActive);
        document.Categories.Should().BeEquivalentTo(genre.Categories.Select(c => new { c.Id, c.Name }));
        _mockServer.FindLogEntries(adminCatalogRequest)
            .Should().HaveCount(1);
        _mockServer.FindLogEntries(_fixture.AuthRequestBuilderMock)
            .Should().HaveCountLessOrEqualTo(1);
    }

    [Fact(DisplayName = nameof(GenreEvent_WhenOperationIsUpdate_SavesGenre))]
    [Trait("E2E/Consumers", "Genre")]
    public async Task GenreEvent_WhenOperationIsUpdate_SavesGenre()
    {
        var examplesList = _fixture.GetGenreModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage<GenrePayloadModel>("u", example);
        var genre = _fixture.GetValidGenre(message.Payload.After.Id);
        var apiResponse = new DataWrapper<Domain.Entity.Genre>(genre);
        var apiResponseBody = JsonSerializer.Serialize(apiResponse, SerializerConfiguration.SnakeCaseSerializerOptions);
        var adminCatalogRequest = Request.Create()
            .WithPath($"/genres/{genre.Id}")
            .WithHeader("Authorization", "Bearer *")
            .UsingGet();
        var adminCatalogResponse = Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(apiResponseBody);
        _mockServer.Given(adminCatalogRequest)
            .RespondWith(adminCatalogResponse);

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await _fixture.ElasticClient
            .GetAsync<GenreModel>(genre.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(genre.Id);
        document.Name.Should().Be(genre.Name);
        document.CreatedAt.Should().Be(genre.CreatedAt);
        document.IsActive.Should().Be(genre.IsActive);
        document.Categories.Should().BeEquivalentTo(genre.Categories.Select(c => new { c.Id, c.Name }));
        _mockServer.FindLogEntries(adminCatalogRequest)
            .Should().HaveCount(1);
        _mockServer.FindLogEntries(_fixture.AuthRequestBuilderMock)
            .Should().HaveCountLessOrEqualTo(1);
    }

    [Fact(DisplayName = nameof(GenreEvent_WhenOperationIsDelete_DeletesGenre))]
    [Trait("E2E/Consumers", "Genre")]
    public async Task GenreEvent_WhenOperationIsDelete_DeletesGenre()
    {
        var examplesList = _fixture.GetGenreModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage<GenrePayloadModel>("d", example);
        var genre = message.Payload.Before;

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(10_000);

        var persisted = await _fixture.ElasticClient
            .GetAsync<GenreModel>(genre.Id);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose()
    {
        _mockServer.Dispose();
        _fixture.DeleteAll();
    }
}