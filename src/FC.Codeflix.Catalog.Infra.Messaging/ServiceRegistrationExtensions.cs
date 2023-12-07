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
            .AddScoped<SaveCategoryMessageHandler>()
            .AddScoped<DeleteCategoryMessageHandler>()
            .AddHostedService<KafkaConsumer<CategoryPayloadModel>>(
                provider =>
                {
                    var configuration = provider.GetRequiredService<IOptions<KafkaConfiguration>>();
                    var logger = provider.GetRequiredService<ILogger<KafkaConsumer<CategoryPayloadModel>>>();
                    var consumer = new KafkaConsumer<CategoryPayloadModel>(
                        configuration.Value.CategoryConsumer, logger, provider);
                    consumer.AddMessageHandler<SaveCategoryMessageHandler>(
                        message => message.Payload.Operation is
                            MessageModelOperation.Create or 
                            MessageModelOperation.Read or 
                            MessageModelOperation.Update);
                    consumer.AddMessageHandler<DeleteCategoryMessageHandler>(
                        message => message.Payload.Operation is MessageModelOperation.Delete);
                    return consumer;
                });
    }
}