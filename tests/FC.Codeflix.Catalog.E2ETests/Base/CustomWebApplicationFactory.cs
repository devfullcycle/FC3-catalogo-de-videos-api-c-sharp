using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

namespace FC.Codeflix.Catalog.E2ETests.Base;
public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public CatalogClient GraphQLClient { get; private set; }
    public readonly string BaseUrl = "http://localhost:61000/";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var environment = "EndToEndTest";
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        builder.UseEnvironment(environment);
        // builder.ConfigureServices(services =>
        // {
        //     services.AddTransient<HttpMessageHandlerBuilder>(
        //         sp => new TestServerHttpMessageHandlerBuilder(Server));
        //     services
        //         .AddCatalogClient()
        //         .ConfigureHttpClient(client => client.BaseAddress = new Uri($"{BaseUrl}graphql"));
        // });
        base.ConfigureWebHost(builder);
    }
    
    // private CatalogClient BuildGraphqlClient()
    // {
    //     var services = new ServiceCollection();
    //     services.AddTransient<HttpMessageHandlerBuilder>(
    //         sp => new TestServerHttpMessageHandlerBuilder(Server));
    //     services
    //         .AddCatalogClient()
    //         .ConfigureHttpClient(client => client.BaseAddress = new Uri($"{BaseUrl}graphql"));
    //     var provider = services.BuildServiceProvider();
    //     return provider.GetRequiredService<CatalogClient>();
    // }
}
