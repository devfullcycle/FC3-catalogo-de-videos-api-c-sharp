using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.Base.Fixture;

public class GenreTestFixtureBase : FixtureBase,IDisposable
{
    public CustomWebApplicationFactory<Program> WebAppFactory { get; }
    public IElasticClient ElasticClient { get; }
    public GenreDataGenerator DataGenerator { get; }
    
    protected GenreTestFixtureBase()
    {
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });
        DataGenerator = new GenreDataGenerator();
        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();   
        ElasticClient.CreateGenreIndexAsync().GetAwaiter().GetResult();
    }

    public IList<GenreModel> GetGenreModelList(int count = 10)
        => DataGenerator.GetGenreModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<GenreModel>();

    public void Dispose()
        => ElasticClient.DeleteGenreIndex();
}