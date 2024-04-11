using FC.Codeflix.Catalog.Application.UseCases.Video.SaveVideo;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Video.Common;
using FluentAssertions;
using NSubstitute;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Video.SaveVideo;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Video.SaveVideo;

[Collection(nameof(VideoUseCaseTestFixture))]
public class SaveVideoUseCaseTest
{
    private readonly VideoUseCaseTestFixture _fixture;

    public SaveVideoUseCaseTest(VideoUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveValidVideo))]
    [Trait("Application", "[UseCase] SaveVideo")]
    public async Task SaveValidVideo()
    {
        var repository = _fixture.GetMockRepository();
        var gateway = _fixture.GetMockAdminCatalogGateway();
        var video = _fixture.GetValidVideo();
        gateway.GetVideoAsync(
                video.Id,
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(video));
        var useCase = new UseCase.SaveVideo(repository, gateway);
        var input = new SaveVideoInput(video.Id);
        
        var output = await useCase.Handle(input, CancellationToken.None);
        
        await repository
            .Received(1)
            .SaveAsync(video, Arg.Any<CancellationToken>());
        output.Should().NotBeNull();
        video.Id.Should().Be(video.Id);
        video.Title.Should().Be(video.Title);
        video.Description.Should().Be(video.Description);
        video.YearLaunched.Should().Be(video.YearLaunched);
        video.Duration.Should().Be(video.Duration);
        video.CreatedAt.Should().Be(video.CreatedAt);
        video.CreatedAt.Should().Be(video.CreatedAt);
        video.Categories.Should().BeEquivalentTo(video.Categories.Select(x => new { x.Id, x.Name }));
        video.Genres.Should().BeEquivalentTo(video.Genres.Select(x => new { x.Id, x.Name }));
        video.CastMembers.Should().BeEquivalentTo(video.CastMembers.Select(x => new { x.Id, x.Name }));

    }
}