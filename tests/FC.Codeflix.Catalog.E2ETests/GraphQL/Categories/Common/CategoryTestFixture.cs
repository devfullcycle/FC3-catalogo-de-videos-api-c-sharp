using FC.Codeflix.Catalog.E2ETests.Base.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;
public class CategoryTestFixture : CategoryTestFixtureBase
{
    public CatalogClient GraphQLClient { get; }
    public CategoryTestFixture()
    {
        GraphQLClient = WebAppFactory.GraphQLClient;
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{ }
