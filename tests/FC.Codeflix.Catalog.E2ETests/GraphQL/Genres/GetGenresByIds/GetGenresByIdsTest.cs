using FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.Common;
using FluentAssertions;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.GetGenresByIds;

[Collection(nameof(GenreTestFixture))]
public class GetGenresByIdsTest : IDisposable
{
    private readonly GenreTestFixture _fixture;

    public GetGenresByIdsTest(GenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetGenresByIds_WhenReceivesValidIds_ReturnGenres))]
    [Trait("E2E/GraphQL", "[Genre] GetByIds")]
    public async Task GetGenresByIds_WhenReceivesValidIds_ReturnGenres()
    {
        var elasticClient = _fixture.ElasticClient;
        var examples = _fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync();
        var genre1 = examples[3];
        var genre2 = examples[5];

        var output = await _fixture.GraphQLClient.GetGenresByIds
            .ExecuteAsync(genre1.Id, genre2.Id, CancellationToken.None);

        output.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Data!.Genre1.Name.Should().Be(genre1.Name);
        output.Data.Genre1.IsActive.Should().Be(genre1.IsActive);
        output.Data.Genre1.CreatedAt.Date.Should().Be(genre1.CreatedAt.Date);
        output.Data.Genre1.Categories.Should().BeEquivalentTo(genre1.Categories);
        output.Data.Genre2.Name.Should().Be(genre2.Name);
    }

    public void Dispose() => _fixture.DeleteAll();
}