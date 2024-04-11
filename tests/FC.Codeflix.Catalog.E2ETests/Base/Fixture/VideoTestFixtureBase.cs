using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.Base.Fixture;

public class VideoTestFixtureBase: FixtureBase,IDisposable
{
    public CustomWebApplicationFactory<Program> WebAppFactory { get; }
    public IElasticClient ElasticClient { get; }
    public VideoDataGenerator DataGenerator { get; }
    
    protected VideoTestFixtureBase()
    {
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });
        DataGenerator = new VideoDataGenerator();
        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();   
        ElasticClient.CreateVideoIndexAsync().GetAwaiter().GetResult();
    }

    public IList<VideoModel> GetVideoModelList(int count = 10)
        => DataGenerator.GetVideoModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<VideoModel>();

    public void Dispose()
        => ElasticClient.DeleteVideoIndex();
}