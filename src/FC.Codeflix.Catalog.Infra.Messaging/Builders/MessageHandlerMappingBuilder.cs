using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging.Builders;

public class MessageHandlerMappingBuilder<TMessage>
    where TMessage : class
{
    private Type _handlerType = null!;
    private Func<MessageModel<TMessage>, bool> _predicate = null!;
    private readonly KafkaConsumerBuilder<TMessage> _kafkaConsumerBuilder;

    public MessageHandlerMappingBuilder(
        KafkaConsumerBuilder<TMessage> kafkaConsumerBuilder)
    {
        _kafkaConsumerBuilder = kafkaConsumerBuilder;
    }

    public KafkaConsumerBuilder<TMessage> And => _kafkaConsumerBuilder;

    public MessageHandlerMappingBuilder<TMessage> With<THandler>()
        where THandler : IMessageHandler<TMessage>
    {
        _handlerType = typeof(THandler);
        return this;
    }

    public MessageHandlerMappingBuilder<TMessage> WithDefault<THandler>()
        where THandler : IMessageHandler<TMessage>
    {
        _handlerType = typeof(THandler);
        _predicate = _ => true;
        return this;
    }

    public MessageHandlerMappingBuilder<TMessage> When(
        Func<MessageModel<TMessage>, bool> predicate)
    {
        _predicate = predicate;
        return this;
    }

    public (Func<MessageModel<TMessage>, bool> Predicate, Type Type) Build()
    {
        return (_predicate, _handlerType);
    }

    public IServiceCollection Register() => _kafkaConsumerBuilder.Register();
}