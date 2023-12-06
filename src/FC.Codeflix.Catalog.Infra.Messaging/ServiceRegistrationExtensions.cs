using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            .AddHostedService<KafkaConsumer<CategoryPayloadModel>>(
                provider =>
                {
                    var configuration = provider.GetRequiredService<IOptions<KafkaConfiguration>>();
                    var logger = provider.GetRequiredService<ILogger<KafkaConsumer<CategoryPayloadModel>>>();
                    return new KafkaConsumer<CategoryPayloadModel>(
                        configuration.Value.CategoryConsumer, logger, provider);
                });
    }
}