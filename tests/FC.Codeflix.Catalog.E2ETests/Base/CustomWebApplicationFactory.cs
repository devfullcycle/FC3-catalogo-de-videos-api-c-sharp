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
    public readonly string BaseUrl = "http://localhost:5555/";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var environment = "EndToEndTest";
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        builder.UseEnvironment(environment);
        builder.ConfigureServices(services =>
        {
            services.AddTransient<HttpMessageHandlerBuilder>(
                sp => new TestServerHttpMessageHandlerBuilder(Server));
            services
                .AddCatalogClient()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri($"{BaseUrl}graphql"));
        });
        base.ConfigureWebHost(builder);
    }
}
