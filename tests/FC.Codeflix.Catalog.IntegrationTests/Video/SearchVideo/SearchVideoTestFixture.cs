using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Video.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Video.SearchVideo;

public class SearchVideoTestFixture: VideoTestFixture
{
    public IList<VideoModel> GetVideoModelList(IEnumerable<string> titles)
        => DataGenerator.GetVideoModelList(titles);
    
    public IList<VideoModel> CloneVideosListOrdered(List<VideoModel> examples, string orderBy, SearchOrder inputOrder)
        => DataGenerator.CloneVideosListOrdered(examples, orderBy, inputOrder);
}

[CollectionDefinition(nameof(SearchVideoTestFixture))]
public class SearchVideoTestFixtureCollection
    : ICollectionFixture<SearchVideoTestFixture>
{}