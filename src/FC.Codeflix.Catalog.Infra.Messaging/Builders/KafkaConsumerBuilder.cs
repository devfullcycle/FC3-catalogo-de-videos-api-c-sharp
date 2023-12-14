using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;
using FC.Codeflix.Catalog.Infra.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FC.Codeflix.Catalog.Infra.Messaging.Builders;

public class KafkaConsumerBuilder<TMessage>
    where TMessage : class
{
    private readonly KafkaConsumerConfiguration _mainConfiguration = new();
    private readonly List<MessageHandlerMappingBuilder<TMessage>> _handlerMapping = new();
    private int _retryCount;
    private readonly IServiceCollection _services;

    public KafkaConsumerBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public KafkaConsumerBuilder<TMessage> Configure(IConfigurationSection section)
    {
        section.Bind(_mainConfiguration);    
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
    
    
    public KafkaConsumer<TMessage> Build(
        IServiceProvider provider,
        KafkaConsumerConfiguration configuration)
    {
        var logger = provider.GetRequiredService<ILogger<KafkaConsumer<TMessage>>>();
        var consumer = new KafkaConsumer<TMessage>(
            configuration, logger, provider);
        _handlerMapping.ForEach(builder =>
        {
            var tuple = builder.Build();
            consumer.AddMessageHandler(tuple.Type, tuple.Predicate);
        });
        return consumer;
    }

    public IServiceCollection Register()
    {
        _mainConfiguration.DlqTopic = _mainConfiguration.Topic.ToDlqTopic();
        if (_retryCount > 0)
        {
            _mainConfiguration.RetryTopic = _mainConfiguration.Topic.ToRetryTopic(1);
        }
        _services.AddSingleton<IHostedService>(provider => Build(provider, _mainConfiguration));
        
        Enumerable.Range(1, _retryCount)
            .ToList()
            .ForEach(index =>
            {
                var hasNextRetry = index < _retryCount;
                var configuration = _mainConfiguration.CreateRetryConfiguration(index, hasNextRetry);
                _services.AddSingleton<IHostedService>(provider => Build(provider, configuration));
            });
        return _services;
    }

    public KafkaConsumerBuilder<TMessage> WithRetries(int retryCount = 3)
    {
        _retryCount = retryCount;
        return this;
    }
}