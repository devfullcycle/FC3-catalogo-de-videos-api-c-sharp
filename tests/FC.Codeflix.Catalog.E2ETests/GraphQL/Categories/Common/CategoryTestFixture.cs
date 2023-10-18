using FC.Codeflix.Catalog.E2ETests.Base;
using FC.Codeflix.Catalog.E2ETests.Base.Fixture;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;
public class CategoryTestFixture : CategoryTestFixtureBase
{
    public CatalogClient GraphQLClient { get; }
    public CategoryTestFixture()
    {
        GraphQLClient = WebAppFactory.Services.GetRequiredService<CatalogClient>();
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{ }
