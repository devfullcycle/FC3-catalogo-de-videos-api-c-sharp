using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Common;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.IntegrationTests.Video.Common;

public class VideoTestFixture: BaseFixture, IDisposable
{
    public IElasticClient ElasticClient { get; }
    public VideoDataGenerator DataGenerator { get; }
    
    public VideoTestFixture()
    {
        ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();
        DataGenerator = new VideoDataGenerator();
        ElasticClient.CreateVideoIndexAsync().GetAwaiter().GetResult();
    }
    
    public void DeleteAll()
        => ElasticClient.DeleteDocuments<VideoModel>();

    public List<VideoModel> GetVideoModelList(int count = 10)
        => DataGenerator.GetVideoModelList(count);

    public void Dispose()
        => ElasticClient.DeleteVideoIndex();
}

[CollectionDefinition(nameof(VideoTestFixture))]
public class VideoTestFixtureCollection 
    : ICollectionFixture<VideoTestFixture>
{ }