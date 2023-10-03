using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Common;
public abstract class BaseFixture
{
    public IServiceProvider ServiceProvider { get; }
    protected BaseFixture()
    {
        ServiceProvider = BuildServiceProvider();
    }

    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>()
        {
            { "ConnectionStrings:ElasticSearch", "http://localhost:9201" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        services
            .AddUseCases()
            .AddElasticSearch(configuration)
            .AddRepositories();

        return services.BuildServiceProvider();
    }
}
