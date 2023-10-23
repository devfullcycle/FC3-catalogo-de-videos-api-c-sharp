using Confluent.Kafka;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
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
            // Processar a mensagem
        }
        consumer.Close();
    }
}