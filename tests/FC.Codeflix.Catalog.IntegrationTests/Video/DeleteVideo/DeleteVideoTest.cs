using FC.Codeflix.Catalog.Application.UseCases.Video.DeleteVideo;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Video.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.Video.DeleteVideo;

[Collection(nameof(VideoTestFixture))]
public class DeleteVideoTest : IDisposable
{
    private readonly VideoTestFixture _fixture;

    public DeleteVideoTest(VideoTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteVideo_WhenReceivesAnExistingId_DeletesVideo))]
    [Trait("Integration", "[UseCase] DeleteVideo")]
    public async Task DeleteVideo_WhenReceivesAnExistingId_DeletesVideo()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var videosExample = _fixture.GetVideoModelList();
        await elasticClient.IndexManyAsync(videosExample);
        var input = new DeleteVideoInput(videosExample[3].Id);

        await mediator.Send(input, CancellationToken.None);

        var deletedVideo = await elasticClient.GetAsync<VideoModel>(input.Id);
        deletedVideo.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteVideo_WhenReceivesANonExistingId_ThrowsException))]
    [Trait("Integration", "[UseCase] DeleteVideo")]
    public async Task DeleteVideo_WhenReceivesANonExistingId_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var videosExample = _fixture.GetVideoModelList();
        await elasticClient.IndexManyAsync(videosExample);
        var input = new DeleteVideoInput(Guid.NewGuid());

        var action = async () => await mediator.Send(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Video '{input.Id}' not found.");
    }

    public void Dispose() => _fixture.DeleteAll();
}