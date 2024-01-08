using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Infra.HttpClients.DelegatingHandlers;
using FC.Codeflix.Catalog.Infra.HttpClients.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.HttpClients;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<AuthenticationClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["HttpClients:AuthenticationServer"]!);
        }); 
        
        services
            .AddScoped<AuthenticationHandler>()
            .AddHttpClient<IAdminCatalogGateway, AdminCatalogClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["HttpClients:AdminCatalogBaseUrl"]!);
            })
            .AddHttpMessageHandler<AuthenticationHandler>();
        return services;
    }  
}