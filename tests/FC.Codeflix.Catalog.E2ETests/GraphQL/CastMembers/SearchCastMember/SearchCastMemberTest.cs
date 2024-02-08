using FC.Codeflix.Catalog.Infra.Data.ES;
using FluentAssertions;
using Nest;
using RepositoryDTOs = FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.CastMembers.SearchCastMember;
[Collection(nameof(SearchCastMemberTestFixture))]
public class SearchCastMemberTest : IDisposable
{
    private readonly SearchCastMemberTestFixture _fixture;

    public SearchCastMemberTest(SearchCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(SearchCastMember_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("E2E/GraphQL", "[CastMember] Search")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchCastMember_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var elasticClient = _fixture.ElasticClient;
        var castMemberNamesList = new List<string>() {
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
        var examples = _fixture.GetCastMemberModelList(castMemberNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.CastMember);

        var output = await _fixture.GraphQLClient.SearchCastMember
            .ExecuteAsync(page, perPage, search, "", SearchOrder.Asc, CancellationToken.None);

        output.Data!.CastMembers.Should().NotBeNull();
        output.Data!.CastMembers.Items.Should().NotBeNull();
        output.Data!.CastMembers.CurrentPage.Should().Be(page);
        output.Data!.CastMembers.PerPage.Should().Be(perPage);
        output.Data!.CastMembers.Total.Should().Be(expectedTotalCount);
        output.Data!.CastMembers.Items.Should().HaveCount(expectedItemsCount);

        foreach (var outputItem in output.Data!.CastMembers.Items)
        {
            var expected = examples.First(x => x.Id == outputItem.Id);
            outputItem.Name.Should().Be(expected!.Name);
            outputItem.Type.ToString("G").Should().Be(expected.Type.ToString("G"));
            outputItem.CreatedAt.Date.Should().Be(expected.CreatedAt.Date);
        }
    }

    [Theory(DisplayName = nameof(SearchCastMember_WhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("E2E/GraphQL", "[CastMember] Search")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchCastMember_WhenReceivesValidSearchInput_ReturnOrderedList(
        string orderBy,
        string direction)
    {
        var elasticClient = _fixture.ElasticClient;
        var examples = _fixture.GetCastMemberModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.CastMember);
        const int page = 1;
        var perPage = examples.Count;
        var directionGraphql = direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var directionRepository = direction == "asc" ?
            RepositoryDTOs.SearchOrder.Asc :
            RepositoryDTOs.SearchOrder.Desc;

        var expectedList = _fixture.CloneCastMembersListOrdered(
            examples, orderBy, directionRepository);

        var output = await _fixture.GraphQLClient.SearchCastMember
            .ExecuteAsync(page, perPage, "", orderBy, directionGraphql);

        output.Data!.CastMembers.Should().NotBeNull();
        output.Data!.CastMembers.Items.Should().NotBeNullOrEmpty();
        output.Data!.CastMembers.CurrentPage.Should().Be(page);
        output.Data!.CastMembers.PerPage.Should().Be(perPage);
        output.Data!.CastMembers.Total.Should().Be(examples.Count);
        output.Data!.CastMembers.Items.Should().HaveCount(examples.Count);
        for (int i = 0; i < output.Data!.CastMembers.Items.Count; i++)
        {
            var outputItem = output.Data!.CastMembers.Items[i];
            var expected = expectedList[i];
            outputItem.Id.Should().Be(expected.Id);
            outputItem.Name.Should().Be(expected.Name);
            outputItem.Type.ToString("G").Should().Be(expected.Type.ToString("G"));
            outputItem.CreatedAt.Date.Should().Be(expected.CreatedAt.Date);
        }
    }

    public void Dispose() => _fixture.DeleteAll();
}