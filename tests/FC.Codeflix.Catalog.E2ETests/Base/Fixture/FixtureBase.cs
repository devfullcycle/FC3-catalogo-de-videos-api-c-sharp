using System.Text.Json;
using Confluent.Kafka;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;

namespace FC.Codeflix.Catalog.E2ETests.Base.Fixture;

public class FixtureBase
{
    protected async Task PublishMessageAsync(string bootstrapServers, string topic, object message)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        using var producer = new ProducerBuilder<string, string>(config).Build();
        var rawMessage = JsonSerializer.Serialize(message, SerializerConfiguration.JsonSerializerOptions);
        _ = await producer.ProduceAsync(
            topic,
            new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = rawMessage
            });
    }
}