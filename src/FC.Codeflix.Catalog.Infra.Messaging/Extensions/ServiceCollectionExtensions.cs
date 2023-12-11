using FC.Codeflix.Catalog.Infra.Messaging.Builders;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static KafkaConsumerBuilder<TMessage> AddKafkaConsumer<TMessage>(
        this IServiceCollection services)
        where TMessage : class
    {
        var builder = new KafkaConsumerBuilder<TMessage>();
        services.AddHostedService<KafkaConsumer<TMessage>>(
            provider => builder.Build(provider));
        return builder;
    }
}