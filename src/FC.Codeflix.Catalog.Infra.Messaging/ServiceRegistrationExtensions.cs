using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers;
using FC.Codeflix.Catalog.Infra.Messaging.Extensions;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.Infra.Messaging;

public static class ServiceRegistrationExtensions
{
    private const string KafkaConfigurationSection = "KafkaConfiguration";
    private const string CategoryConsumerConfigurationSection = "CategoryConsumer";
    public static IServiceCollection AddConsumers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<KafkaConfiguration>()
            .BindConfiguration(KafkaConfigurationSection);

        var kafkaConfiguration = configuration.GetSection(KafkaConfigurationSection);
        
        return services
            .AddScoped<SaveCategoryMessageHandler>()
            .AddScoped<DeleteCategoryMessageHandler>()
            .AddKafkaConsumer<CategoryPayloadModel>()
                .Configure(kafkaConfiguration.GetSection(CategoryConsumerConfigurationSection))
                .WithRetries(3)
                .With<SaveCategoryMessageHandler>()
                .When(message => message.Payload.Operation is
                    MessageModelOperation.Create or
                    MessageModelOperation.Read or
                    MessageModelOperation.Update)
                .And
                .With<DeleteCategoryMessageHandler>()
                .When(message => message.Payload.Operation is MessageModelOperation.Delete)
                .Register();
    }
}