namespace FC.Codeflix.Catalog.Infra.Messaging.Configuration;

public class KafkaConfiguration
{
    public KafkaConsumerConfiguration CategoryConsumer { get; set; } = null!;
}

public class KafkaConsumerConfiguration
{
    public string BootstrapServers { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public string Topic { get; set; } = null!;
}