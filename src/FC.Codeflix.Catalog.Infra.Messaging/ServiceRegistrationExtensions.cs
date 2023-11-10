using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddConsumers(
        this IServiceCollection services)
    {
        services.AddOptions<KafkaConfiguration>()
            .BindConfiguration("KafkaConfiguration");
        return services.AddHostedService<CategoryConsumer>();
    }
}