using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;
using Microsoft.Extensions.Configuration;

namespace FC.Codeflix.Catalog.Infra.Messaging.Builders;

public class KafkaConsumerBuilder<TMessage>
    where TMessage : class
{
    private readonly KafkaConsumerConfiguration _configuration = new();
    public KafkaConsumer<TMessage> Build(IServiceProvider provider)
    {
        throw new NotImplementedException();
    }

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
        return mappingBuilder;
    }
}