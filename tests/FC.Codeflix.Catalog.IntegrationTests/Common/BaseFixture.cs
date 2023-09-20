using Bogus;
using Microsoft.Extensions.DependencyInjection;
using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;
using Microsoft.Extensions.Configuration;

namespace FC.Codeflix.Catalog.IntegrationTests.Common;
public abstract class BaseFixture
{
    public IServiceProvider ServiceProvider { get; }
    public Faker Faker { get; set; }

    protected BaseFixture()
    {
        Faker = new Faker("pt_BR");
        ServiceProvider = BuildServiceProvider();
    }

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;

    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>()
        {
            { "ConnectionStrings:ElasticSearch", "http://localhost:9200" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        services
            .AddUseCases()
            .AddElasticSearch(configuration)
            .AddRepositories();

        return services.BuildServiceProvider();
    }
}
