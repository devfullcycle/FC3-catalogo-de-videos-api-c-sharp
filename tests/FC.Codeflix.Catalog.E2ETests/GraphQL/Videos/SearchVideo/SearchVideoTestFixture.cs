using FC.Codeflix.Catalog.E2ETests.Base.Fixture;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using RepositoryDTOs = FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Videos.SearchVideo;

public class SearchVideoTestFixture: VideoTestFixtureBase
{
    public CatalogClient GraphQLClient { get; }
    public SearchVideoTestFixture()
    {
        GraphQLClient = WebAppFactory.GraphQLClient;
    }
    
    public IList<VideoModel> GetVideoModelList(IEnumerable<string> videoNames)
        => DataGenerator.GetVideoModelList(videoNames);

    public IList<VideoModel> CloneVideosListOrdered(
        List<VideoModel> videosList,
        string orderBy,
        RepositoryDTOs.SearchOrder direction)
        => DataGenerator.CloneVideosListOrdered(videosList, orderBy, direction);
}

[CollectionDefinition(nameof(SearchVideoTestFixture))]
public class SearchVideoTestFixtureCollection
    : ICollectionFixture<SearchVideoTestFixture>
{ }
