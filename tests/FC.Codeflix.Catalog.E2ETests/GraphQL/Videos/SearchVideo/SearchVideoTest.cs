using FC.Codeflix.Catalog.Infra.Data.ES;
using FluentAssertions;
using Nest;
using RepositoryDTOs = FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Videos.SearchVideo;

[Collection(nameof(SearchVideoTestFixture))]
public class SearchVideoTest : IDisposable
{
    private readonly SearchVideoTestFixture _fixture;

    public SearchVideoTest(SearchVideoTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Theory(DisplayName = nameof(SearchVideo_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("E2E/GraphQL", "[Video] Search")]
    [InlineData("007", 2, 2, 2, 4)]
    [InlineData("Star", 1, 2, 2, 2)]
    [InlineData("Casino Royal", 1, 2, 1, 1)]
    [InlineData("Terminator", 1, 5, 0, 0)]
    public async Task SearchVideo_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var elasticClient = _fixture.ElasticClient;
        var videoNamesList = new List<string>() {
            "007: Dr. No",
            "007: Casino Royale",
            "007: GoldFinger",
            "007: Skyfall",
            "Star Wars: Return of the Jedi",
            "Star Wars: The Empire Strikes Back",
            "Interstellar"
        };
        var examples = _fixture.GetVideoModelList(videoNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Video);

        var output = await _fixture.GraphQLClient.SearchVideo
            .ExecuteAsync(page, perPage, search, "", SearchOrder.Asc, CancellationToken.None);

        output.Data!.Videos.Should().NotBeNull();
        output.Data!.Videos.Items.Should().NotBeNull();
        output.Data!.Videos.CurrentPage.Should().Be(page);
        output.Data!.Videos.PerPage.Should().Be(perPage);
        output.Data!.Videos.Total.Should().Be(expectedTotalCount);
        output.Data!.Videos.Items.Should().HaveCount(expectedItemsCount);

        foreach (var outputItem in output.Data!.Videos.Items)
        {
            var expected = examples.First(x => x.Id == outputItem.Id);
            outputItem.Title.Should().Be(expected.Title);
            outputItem.Description.Should().Be(expected.Description);
            outputItem.YearLaunched.Should().Be(expected.YearLaunched);
            outputItem.Duration.Should().Be(expected.Duration);
            outputItem.Rating.ToString().ToLower().Should().Be(expected.Rating.ToString().ToLower());
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
            outputItem.Genres.Should().BeEquivalentTo(expected.Genres);
            outputItem.CastMembers.Should().BeEquivalentTo(expected.CastMembers);
        }
    }

    [Theory(DisplayName = nameof(SearchVideo_WhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("E2E/GraphQL", "[Video] Search")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchVideo_WhenReceivesValidSearchInput_ReturnOrderedList(
        string orderBy,
        string direction)
    {
        var elasticClient = _fixture.ElasticClient;
        var examples = _fixture.GetVideoModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Video);
        const int page = 1;
        var perPage = examples.Count;
        var directionGraphql = direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var directionRepository = direction == "asc" ?
            RepositoryDTOs.SearchOrder.Asc :
            RepositoryDTOs.SearchOrder.Desc;

        var expectedList = _fixture.CloneVideosListOrdered(
            examples.ToList(), orderBy, directionRepository);

        var output = await _fixture.GraphQLClient.SearchVideo
            .ExecuteAsync(page, perPage, "", orderBy, directionGraphql);

        output.Data!.Videos.Should().NotBeNull();
        output.Data!.Videos.Items.Should().NotBeNullOrEmpty();
        output.Data!.Videos.CurrentPage.Should().Be(page);
        output.Data!.Videos.PerPage.Should().Be(perPage);
        output.Data!.Videos.Total.Should().Be(examples.Count);
        output.Data!.Videos.Items.Should().HaveCount(examples.Count);
        for (int i = 0; i < output.Data!.Videos.Items.Count; i++)
        {
            var outputItem = output.Data!.Videos.Items[i];
            var expected = expectedList[i];
            outputItem.Title.Should().Be(expected.Title);
            outputItem.Description.Should().Be(expected.Description);
            outputItem.YearLaunched.Should().Be(expected.YearLaunched);
            outputItem.Duration.Should().Be(expected.Duration);
            outputItem.Rating.ToString().ToLower().Should().Be(expected.Rating.ToString().ToLower());
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
            outputItem.Genres.Should().BeEquivalentTo(expected.Genres);
            outputItem.CastMembers.Should().BeEquivalentTo(expected.CastMembers);
        }
    }

    public void Dispose() => _fixture.DeleteAll();
}