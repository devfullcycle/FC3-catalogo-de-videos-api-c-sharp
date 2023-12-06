using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddConsumers(
        this IServiceCollection services)
    {
        services.AddOptions<KafkaConfiguration>()
            .BindConfiguration("KafkaConfiguration");
        return services
            .AddScoped<IMessageHandler<CategoryPayloadModel>, CategoryMessageHandler>()
            .AddHostedService<CategoryConsumer>();
    }
}