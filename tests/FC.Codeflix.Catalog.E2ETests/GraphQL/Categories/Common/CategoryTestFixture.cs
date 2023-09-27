using FC.Codeflix.Catalog.E2ETests.Base;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;
public class CategoryTestFixture
{
    public CustomWebApplicationFactory<Program> WebAppFactory { get; private set; } = null!;
    public CatalogClient GraphQLClient { get; private set; } = null!;

    public CategoryTestFixture()
    {
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });
        GraphQLClient = WebAppFactory.Services.GetRequiredService<CatalogClient>();
    }
}
