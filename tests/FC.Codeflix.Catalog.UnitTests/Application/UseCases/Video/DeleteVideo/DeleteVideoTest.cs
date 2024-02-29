using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Video.Common;
using NSubstitute;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Video.DeleteVideo;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Video.DeleteVideo;

[Collection(nameof(VideoUseCaseTestFixture))]
public class DeleteVideoTest
{
    private readonly VideoUseCaseTestFixture _fixture;

    public DeleteVideoTest(VideoUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteVideo))]
    [Trait("Application", "[UseCase] DeleteVideo")]
    public async Task DeleteVideo()
    {
        var repository = _fixture.GetMockRepository();
        var useCase = new UseCase.DeleteVideo(repository);
        var input = new UseCase.DeleteVideoInput(Guid.NewGuid());

        await useCase.Handle(input, CancellationToken.None);

        await repository.Received(1)
            .DeleteAsync(input.Id, Arg.Any<CancellationToken>());
    }
}