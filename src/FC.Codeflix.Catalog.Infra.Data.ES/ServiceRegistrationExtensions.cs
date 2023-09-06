using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.Codeflix.Catalog.Infra.Data.ES;
public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddElasticSearch(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration
            .GetConnectionString("ElasticSearch");
        var uri = new Uri(connectionString!);
        var connectionSettings = new ConnectionSettings(uri)
            //.DefaultMappingFor<CategoryModel>(i => i
            //    .IndexName(ElasticsearchIndexes.Category)
            //    .IdProperty(p => p.Id)
            //)
            //.EnableDebugMode()
            .PrettyJson()
            .ThrowExceptions()
            .RequestTimeout(TimeSpan.FromMinutes(2));

        var client = new ElasticClient(connectionSettings);
        services.AddSingleton<IElasticClient>(client);
        return services;
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {

        return services;
    }

}
