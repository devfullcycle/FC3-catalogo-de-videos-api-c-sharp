using FC.Codeflix.Catalog.Infra.Data.ES;
using FluentAssertions;
using Nest;
using RepositoryDTOs = FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.SearchGenre;

[Collection(nameof(SearchGenreTestFixture))]
public class SearchGenreTest
{
    private readonly SearchGenreTestFixture _fixture;

    public SearchGenreTest(SearchGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Theory(DisplayName = nameof(SearchGenre_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("E2E/GraphQL", "[Genre] Search")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchGenre_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var elasticClient = _fixture.ElasticClient;
        var genreNamesList = new List<string>() {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        };
        var examples = _fixture.GetGenreModelList(genreNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);

        var output = await _fixture.GraphQLClient.SearchGenre
            .ExecuteAsync(page, perPage, search, "", SearchOrder.Asc, CancellationToken.None);

        output.Data!.Genres.Should().NotBeNull();
        output.Data!.Genres.Items.Should().NotBeNull();
        output.Data!.Genres.CurrentPage.Should().Be(page);
        output.Data!.Genres.PerPage.Should().Be(perPage);
        output.Data!.Genres.Total.Should().Be(expectedTotalCount);
        output.Data!.Genres.Items.Should().HaveCount(expectedItemsCount);

        foreach (var outputItem in output.Data!.Genres.Items)
        {
            var expected = examples.First(x => x.Id == outputItem.Id);
            outputItem.Name.Should().Be(expected!.Name);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }

    [Theory(DisplayName = nameof(SearchGenre_WhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("E2E/GraphQL", "[Genre] Search")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchGenre_WhenReceivesValidSearchInput_ReturnOrderedList(
        string orderBy,
        string direction)
    {
        var elasticClient = _fixture.ElasticClient;
        var examples = _fixture.GetGenreModelList().ToList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);
        const int page = 1;
        var perPage = examples.Count;
        var directionGraphql = direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var directionRepository = direction == "asc" ?
            RepositoryDTOs.SearchOrder.Asc :
            RepositoryDTOs.SearchOrder.Desc;

        var expectedList = _fixture.CloneGenresListOrdered(
            examples, orderBy, directionRepository);

        var output = await _fixture.GraphQLClient.SearchGenre
            .ExecuteAsync(page, perPage, "", orderBy, directionGraphql);

        output.Data!.Genres.Should().NotBeNull();
        output.Data!.Genres.Items.Should().NotBeNullOrEmpty();
        output.Data!.Genres.CurrentPage.Should().Be(page);
        output.Data!.Genres.PerPage.Should().Be(perPage);
        output.Data!.Genres.Total.Should().Be(examples.Count);
        output.Data!.Genres.Items.Should().HaveCount(examples.Count);
        for (int i = 0; i < output.Data!.Genres.Items.Count; i++)
        {
            var outputItem = output.Data!.Genres.Items[i];
            var expected = expectedList[i];
            outputItem.Id.Should().Be(expected.Id);
            outputItem.Name.Should().Be(expected.Name);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }

    public void Dispose() => _fixture.DeleteAll();
}