using System.Text.Json;
using Confluent.Kafka;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers;

public class KafkaConsumer<TMessage> : BackgroundService
    where TMessage : class
{
    private readonly KafkaConsumerConfiguration _configuration;
    private readonly ILogger<KafkaConsumer<TMessage>> _logger;
    private readonly IServiceProvider _serviceProvider;

    private readonly List<(Func<MessageModel<TMessage>, bool> Predicate, Type Type)> _messageHandlers = new();

    public KafkaConsumer(
        KafkaConsumerConfiguration configuration,
        ILogger<KafkaConsumer<TMessage>> logger,
        IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    private ConsumerConfig GetConsumerConfig()
        => new()
        {
            BootstrapServers = _configuration.BootstrapServers,
            GroupId = _configuration.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            EnableAutoOffsetStore = false
        };
 
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = GetConsumerConfig();
        var topic = _configuration.Topic;
        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = await Task.Run(() =>
                consumer.Consume((int)TimeSpan.FromSeconds(30).TotalMilliseconds), stoppingToken);
            if (consumeResult == null || consumeResult.IsPartitionEOF || stoppingToken.IsCancellationRequested)
            {
                continue;
            }
            await HandleMessageAsync(consumeResult.Message, stoppingToken);
            consumer.StoreOffset(consumeResult);
        }
        consumer.Close();
    }

    private async Task HandleMessageAsync(Message<string, string> message, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var messageModel = JsonSerializer.Deserialize<MessageModel<TMessage>>(
            message.Value, SerializerConfiguration.JsonSerializerOptions);

        var messageHandlerType = _messageHandlers
            .FirstOrDefault(tuple => tuple.Predicate.Invoke(messageModel!))
            .Type;

        if (messageHandlerType is null)
        {
            _logger.LogError("None of the message handlers predicates was satisfied by the message: {@message}",
                message);
            return;
        }

        var handler = (IMessageHandler<TMessage>?)scope.ServiceProvider.GetRequiredService(messageHandlerType);

        if (handler is null)
        {
            _logger.LogError("No message handle found of type: {@messageHandlerType}", messageHandlerType);
            return;
        }
        
        await handler.HandleMessageAsync(messageModel!, cancellationToken);
    }

    public void AddMessageHandler<T>(Func<MessageModel<TMessage>, bool> predicate)
    {
        _messageHandlers.Add((predicate, typeof(T)));
    }

    public void AddMessageHandler(Type type, Func<MessageModel<TMessage>, bool> predicate)
    {
        _messageHandlers.Add((predicate, type));
    }
}