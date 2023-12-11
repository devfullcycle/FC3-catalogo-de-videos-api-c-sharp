using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FC.Codeflix.Catalog.Infra.Messaging.Builders;

public class KafkaConsumerBuilder<TMessage>
    where TMessage : class
{
    private readonly KafkaConsumerConfiguration _configuration = new();
    private readonly List<MessageHandlerMappingBuilder<TMessage>> _handlerMapping = new();

    public KafkaConsumerBuilder<TMessage> Configure(IConfigurationSection section)
    {
        section.Bind(_configuration);    
        return this;
    }

    public MessageHandlerMappingBuilder<TMessage> With<THandler>()
        where THandler : IMessageHandler<TMessage>
    {
        var mappingBuilder = new MessageHandlerMappingBuilder<TMessage>(this)
            .With<THandler>();
        _handlerMapping.Add(mappingBuilder);
        return mappingBuilder;
    }
    
    
    public KafkaConsumer<TMessage> Build(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILogger<KafkaConsumer<TMessage>>>();
        var consumer = new KafkaConsumer<TMessage>(
            _configuration, logger, provider);
        _handlerMapping.ForEach(builder =>
        {
            var tuple = builder.Build();
            consumer.AddMessageHandler(tuple.Type, tuple.Predicate);
        });
        return consumer;
    }
}