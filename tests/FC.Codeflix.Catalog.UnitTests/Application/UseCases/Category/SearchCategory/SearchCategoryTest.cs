using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using Moq;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.SearchCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.SearchCategory;
[Collection(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTest
{
    private readonly SearchCategoryTestFixture _fixture;

    public SearchCategoryTest(SearchCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ReturnsSearchResult))]
    [Trait("Application", "[UseCase] SearchCategory")]
    public async Task ReturnsSearchResult()
    {
        var repository = _fixture.GetMockRepository();
        var categories = _fixture.GetCategoryList();
        var input = _fixture.GetSearchInput();
        var expectedQueryResult = new SearchOutput<DomainEntity.Category>(
            input.Page,
            input.PerPage,
            input.PerPage,
            categories);
        repository.Setup(x => x.SearchAsync(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedQueryResult);
        var useCase = new UseCase.SearchCategory(
            repository.Object);

        var output = useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQueryResult.Total);
        output.Items.Should().BeEquivalent(categories);
        repository.Verify(x => x.SearchAsync(
            It.Is<SearchInput>(search =>
                search.Page == input.Page &&
                search.PerPage == input.PerPage &&
                search.Search == input.Search &&
                search.Order == input.Order &&
                search.OrderBy == input.OrderBy),
            It.IsAny<CancellationToken>()),
            Times.Once
        );

    }
}
