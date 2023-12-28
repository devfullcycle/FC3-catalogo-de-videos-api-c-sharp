using Elasticsearch.Net;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Common;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.Common;
public class CategoryTestFixture : BaseFixture, IDisposable
{
    public CategoryDataGenerator DataGenerator { get; }
    public IElasticClient ElasticClient { get; }

    public CategoryTestFixture()
        : base()
    {
        ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();
        DataGenerator = new CategoryDataGenerator();
        ElasticClient.CreateCategoryIndexAsync().GetAwaiter().GetResult();
    }

    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();

    public IList<CategoryModel> GetCategoryModelList(int count = 10)
        => DataGenerator.GetCategoryModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<CategoryModel>();

    public void Dispose()
        => ElasticClient.DeleteCategoryIndex();
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{ }
