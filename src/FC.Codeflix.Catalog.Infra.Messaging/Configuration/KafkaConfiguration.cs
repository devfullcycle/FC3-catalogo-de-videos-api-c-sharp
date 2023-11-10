namespace FC.Codeflix.Catalog.Infra.Messaging.Configuration;

public class KafkaConfiguration
{
    public string BootstrapServers { get; set; } = null!;
    public KafkaConsumerConfiguration CategoryConsumer { get; set; } = null!;
}

public class KafkaConsumerConfiguration
{
    public string GroupId { get; set; } = null!;
    public string Topic { get; set; } = null!;
}