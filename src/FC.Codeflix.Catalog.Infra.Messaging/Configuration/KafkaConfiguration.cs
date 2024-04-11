using FC.Codeflix.Catalog.Infra.Messaging.Extensions;

namespace FC.Codeflix.Catalog.Infra.Messaging.Configuration;

public class KafkaConfiguration
{
    public KafkaConsumerConfiguration CategoryConsumer { get; set; } = null!;
    public KafkaConsumerConfiguration GenreConsumer { get; set; } = null!;
    public KafkaConsumerConfiguration GenreCategoryConsumer { get; set; } = null!;
    public KafkaConsumerConfiguration CastMemberConsumer { get; set; } = null!;
    public KafkaConsumerConfiguration VideoConsumer { get; set; } = null!;
}

public class KafkaConsumerConfiguration
{
    public string BootstrapServers { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public string Topic { get; set; } = null!;
    public string? RetryTopic { get; set; }
    public string? DlqTopic { get; set; }
    public int ConsumeDelaySeconds { get; private set; }

    public bool HasRetry => !string.IsNullOrWhiteSpace(RetryTopic);

    public KafkaConsumerConfiguration CreateRetryConfiguration(int retryIndex, bool hasNextRetry)
        => new()
        {
            BootstrapServers = BootstrapServers,
            GroupId = GroupId,
            Topic = Topic.ToRetryTopic(retryIndex),
            RetryTopic = !hasNextRetry ? null : Topic.ToRetryTopic(retryIndex + 1),
            DlqTopic = DlqTopic,
            ConsumeDelaySeconds = (int)Math.Pow(2, retryIndex)
        };

}