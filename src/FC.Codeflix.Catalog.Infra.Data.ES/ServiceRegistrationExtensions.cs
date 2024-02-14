using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Infra.Data.ES.Repositories;
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
            .DefaultMappingFor<CategoryModel>(i => i
                .IndexName(ElasticsearchIndices.Category)
                .IdProperty(p => p.Id)
            )
            .DefaultMappingFor<GenreModel>(i => i
                .IndexName(ElasticsearchIndices.Genre)
                .IdProperty(p => p.Id)
            )
            .DefaultMappingFor<CastMemberModel>(i => i
                .IndexName(ElasticsearchIndices.CastMember)
                .IdProperty(p => p.Id)
            )
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
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ICastMemberRepository, CastMemberRepository>();
        return services;
    }

}
