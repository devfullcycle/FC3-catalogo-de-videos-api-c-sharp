using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using NSubstitute;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Video.SearchVideo;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Video.SearchVideo;

[Collection(nameof(SearchVideoUseCaseTestFixture))]
public class SearchVideoUseCaseTest
{
    private readonly SearchVideoUseCaseTestFixture _fixture;

    public SearchVideoUseCaseTest(SearchVideoUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ReturnSearchResult))]
    [Trait("Application", "[UseCase] SearchVideo")]
    public async Task ReturnSearchResult()
    {
        var repository = _fixture.GetMockRepository();
        var videos = _fixture.GetVideoList();
        var input = _fixture.GetSearchInput();
        var expectedQueryResult = new SearchOutput<DomainEntity.Video>(
            input.Page,
            input.PerPage,
            input.PerPage,
            videos.ToList());
        repository.SearchAsync(
            Arg.Any<SearchInput>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(expectedQueryResult));
        var expectedOutput = videos
            .Select(video => new
            {
                video.Id,
                video.Title,
                video.Description,
                video.YearLaunched,
                video.CreatedAt,
                video.Rating,
                video.Medias.ThumbUrl,
                video.Medias.ThumbHalfUrl,
                video.Medias.BannerUrl,
                video.Medias.MediaUrl,
                video.Medias.TrailerUrl,
                Categories = video.Categories.Select(category => new { category.Id, category.Name }),
                Genres = video.Genres.Select(genre => new { genre.Id, genre.Name }),
                CastMembers = video.CastMembers.Select(castMember => new { castMember.Id, castMember.Name })
            });
        var useCase = new UseCase.SearchVideo(repository);
        
        var output = await useCase.Handle(input, CancellationToken.None);
        
        output.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQueryResult.Total);
        output.Items.Should().BeEquivalentTo(expectedOutput);
        await repository.Received(1).SearchAsync(
            Arg.Is<SearchInput>(search =>
                search.Page == input.Page &&
                search.PerPage == input.PerPage &&
                search.Search == input.Search &&
                search.Order == input.Order &&
                search.OrderBy == input.OrderBy),
            Arg.Any<CancellationToken>());
    }
}