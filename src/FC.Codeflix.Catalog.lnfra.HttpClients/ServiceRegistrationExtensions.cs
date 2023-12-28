using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.lnfra.HttpClients.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.lnfra.HttpClients;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IAdminCatalogGateway, AdminCatalogClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["HttpClients:AdminCatalogBaseUrl"]!);
        });
        return services;
    }  
}