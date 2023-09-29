using FC.Codeflix.Catalog.E2ETests.Base;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;
public class CategoryTestFixture : IDisposable
{
    public CustomWebApplicationFactory<Program> WebAppFactory { get; }
    public CatalogClient GraphQLClient { get; }
    public IElasticClient ElasticClient { get; }
    public CategoryDataGenerator DataGenerator { get; }
    public CategoryTestFixture()
    {
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });
        DataGenerator = new CategoryDataGenerator();
        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();   
        GraphQLClient = WebAppFactory.Services.GetRequiredService<CatalogClient>();
        ElasticSearchOperations.CreateCategoryIndexAsync(ElasticClient).GetAwaiter().GetResult();
    }

    public void DeleteAll()
        => ElasticSearchOperations.DeleteCategoryDocuments(ElasticClient);

    public void Dispose()
        => ElasticSearchOperations.DeleteCategoryIndex(ElasticClient);
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{ }
