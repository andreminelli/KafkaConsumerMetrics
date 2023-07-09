namespace KafkaConsumerMetrics
{
    public record OffsetData(string ClusterName, string Topic, long TopicCurrentOffset, string ConsumerGroupId, int Partition, long ConsumerOffset);
}
