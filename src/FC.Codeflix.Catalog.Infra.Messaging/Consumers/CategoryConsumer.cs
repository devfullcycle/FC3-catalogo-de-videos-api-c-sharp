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

public class CategoryConsumer : BackgroundService
{
    private readonly KafkaConfiguration _configuration;
    private readonly ILogger<CategoryConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CategoryConsumer(
        IOptions<KafkaConfiguration> configuration,
        ILogger<CategoryConsumer> logger,
        IServiceProvider serviceProvider)
    {
        _configuration = configuration.Value;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    private ConsumerConfig GetConsumerConfig()
        => new()
        {
            BootstrapServers = _configuration.BootstrapServers,
            GroupId = _configuration.CategoryConsumer.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            EnableAutoOffsetStore = false
        };
 
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = GetConsumerConfig();
        var topic = _configuration.CategoryConsumer.Topic;
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
        var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<CategoryPayloadModel>>();

        var messageModel = JsonSerializer.Deserialize<MessageModel<CategoryPayloadModel>>(
            message.Value, SerializerConfiguration.JsonSerializerOptions);
        
        await handler.HandleMessageAsync(messageModel!, cancellationToken);

    }
}