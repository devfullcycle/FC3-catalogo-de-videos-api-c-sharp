using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.Base.Fixture;

public class CategoryTestFixtureBase : IDisposable
{
    public CustomWebApplicationFactory<Program> WebAppFactory { get; }
    public IElasticClient ElasticClient { get; }
    public CategoryDataGenerator DataGenerator { get; }
    
    protected CategoryTestFixtureBase()
    {
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });
        DataGenerator = new CategoryDataGenerator();
        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();   
        ElasticClient.CreateCategoryIndexAsync().GetAwaiter().GetResult();
    }

    public IList<CategoryModel> GetCategoryModelList(int count = 10)
        => DataGenerator.GetCategoryModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<CategoryModel>();

    public void Dispose()
        => ElasticClient.DeleteCategoryIndex();
}