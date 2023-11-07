using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FluentAssertions;
using NSubstitute;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.SearchGenre;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SearchGenre;

[Collection(nameof(SearchGenreUseCaseTestFixture))]
public class SearchGenreUseCaseTest
{
    private readonly SearchGenreUseCaseTestFixture _fixture;

    public SearchGenreUseCaseTest(SearchGenreUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ReturnsSearchResult))]
    [Trait("Application", "[UseCase] SearchGenre")]
    public async Task ReturnsSearchResult()
    {
        var repository = _fixture.GetMockRepository();
        var genres = _fixture.GetGenreList();
        var input = _fixture.GetSearchInput();
        var expectedOutput = genres
            .Select(genre => new
            {
                genre.Id,
                genre.Name,
                genre.CreatedAt,
                genre.IsActive,
                Categories = genre.Categories.Select(category => new { category.Id, category.Name })
            });
        var expectedQueryResult = new SearchOutput<DomainEntity.Genre>(
            input.Page,
            input.PerPage,
            input.PerPage,
            genres);
        repository.SearchAsync(
            Arg.Any<SearchInput>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(expectedQueryResult));
        var useCase = new UseCase.SearchGenre(repository);

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