using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Infra.HttpClients.DelegatingHandlers;
using FC.Codeflix.Catalog.Infra.HttpClients.HttpClients;
using FC.Codeflix.Catalog.Infra.HttpClients.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.HttpClients;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<CredentialsModel>()
            .BindConfiguration("HttpClients:AuthenticationServer:Credentials");
        
        services.AddHttpClient<AuthenticationClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["HttpClients:AuthenticationServer:BaseUrl"]!);
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