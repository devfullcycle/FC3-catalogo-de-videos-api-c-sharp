using System.Text.Json;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Infra.HttpClients.Configuration;
using FluentAssertions;
using Nest;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Video;

[Collection(nameof(VideoConsumerTestFixture))]
public class VideoConsumerTest : IDisposable
{
    private readonly VideoConsumerTestFixture _fixture;
    private readonly WireMockServer _mockServer = WireMockServer.Start(5555);

    public VideoConsumerTest(VideoConsumerTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ConfigureGetTokenMock(_mockServer);
    }

    [Theory(DisplayName = nameof(VideoEvent_WhenOperationIsCreateOrRead_SavesVideo))]
    [Trait("E2E/Consumers", "Video")]
    [InlineData("c")]
    [InlineData("r")]
    public async Task VideoEvent_WhenOperationIsCreateOrRead_SavesVideo(string operation)
    {
        var message = _fixture.BuildValidMessage(operation);
        Domain.Entity.Video video = _fixture.GetValidVideo(message.Payload.After.Id);
        var castMembers = video.CastMembers.Select(CastMemberModel.FromEntity).ToList();
        await _fixture.ElasticClient.IndexManyAsync(castMembers);
        await _fixture.ElasticClient.Indices.RefreshAsync();
        var apiResponse = _fixture.GetValidAdminCatalogApiResponse(video);
        var apiResponseBody = JsonSerializer.Serialize(apiResponse, SerializerConfiguration.SnakeCaseSerializerOptions);
        var adminCatalogRequest = Request.Create()
            .WithPath($"/videos/{video.Id}")
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
            .GetAsync<VideoModel>(video.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Title.Should().Be(video.Title);
        document.Description.Should().Be(video.Description);
        document.YearLaunched.Should().Be(video.YearLaunched);
        document.Duration.Should().Be(video.Duration);
        document.Rating.ToString().ToLower().Should().Be(video.Rating.ToString().ToLower());
        document.CreatedAt.Should().Be(video.CreatedAt);
        document.Categories.Should().BeEquivalentTo(video.Categories.Select(x => new { x.Id, x.Name }));
        document.Genres.Should().BeEquivalentTo(video.Genres.Select(x => new { x.Id, x.Name }));
        document.CastMembers.Should().BeEquivalentTo(castMembers.Select(x => new { x.Id, x.Name, x.Type }));
        _mockServer.FindLogEntries(adminCatalogRequest)
            .Should().HaveCount(1);
        _mockServer.FindLogEntries(_fixture.AuthRequestBuilderMock)
            .Should().HaveCountLessOrEqualTo(1);
    }

    [Fact(DisplayName = nameof(VideoEvent_WhenOperationIsUpdate_SavesVideo))]
    [Trait("E2E/Consumers", "Video")]
    public async Task VideoEvent_WhenOperationIsUpdate_SavesVideo()
    {
        var examplesList = _fixture.GetVideoModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage("u", example.Id);
        var video = _fixture.GetValidVideo(example.Id);
        var castMembers = video.CastMembers.Select(CastMemberModel.FromEntity).ToList();
        await _fixture.ElasticClient.IndexManyAsync(castMembers);
        await _fixture.ElasticClient.Indices.RefreshAsync();
        var apiResponse = _fixture.GetValidAdminCatalogApiResponse(video);
        var apiResponseBody = JsonSerializer.Serialize(apiResponse, SerializerConfiguration.SnakeCaseSerializerOptions);
        var adminCatalogRequest = Request.Create()
            .WithPath($"/videos/{video.Id}")
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
            .GetAsync<VideoModel>(video.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Title.Should().Be(video.Title);
        document.Description.Should().Be(video.Description);
        document.YearLaunched.Should().Be(video.YearLaunched);
        document.Duration.Should().Be(video.Duration);
        document.Rating.ToString().ToLower().Should().Be(video.Rating.ToString().ToLower());
        document.CreatedAt.Should().Be(video.CreatedAt);
        document.Categories.Should().BeEquivalentTo(video.Categories.Select(x => new { x.Id, x.Name }));
        document.Genres.Should().BeEquivalentTo(video.Genres.Select(x => new { x.Id, x.Name }));
        document.CastMembers.Should().BeEquivalentTo(castMembers.Select(x => new { x.Id, x.Name, x.Type }));
        _mockServer.FindLogEntries(adminCatalogRequest)
            .Should().HaveCount(1);
        _mockServer.FindLogEntries(_fixture.AuthRequestBuilderMock)
            .Should().HaveCountLessOrEqualTo(1);
    }

    [Fact(DisplayName = nameof(VideoEvent_WhenOperationIsDelete_DeletesVideo))]
    [Trait("E2E/Consumers", "Video")]
    public async Task VideoEvent_WhenOperationIsDelete_DeletesVideo()
    {
        var examplesList = _fixture.GetVideoModelList();
        await _fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = _fixture.BuildValidMessage("d", example.Id);
        var video = message.Payload.Before;

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await _fixture.ElasticClient
            .GetAsync<VideoModel>(video.Id);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose()
    {
        _mockServer.Dispose();
        _fixture.DeleteAll();
    }
}