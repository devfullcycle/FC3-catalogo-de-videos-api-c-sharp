namespace FC.Codeflix.Catalog.Infra.Messaging.Extensions;

public static class StringExtensions
{
    public static string ToDlqTopic(this string topicName)
        => $"{topicName}-dlq";

    public static string ToRetryTopic(this string topicName, int retryIndex)
        => $"{topicName}-retry-{retryIndex}";
}