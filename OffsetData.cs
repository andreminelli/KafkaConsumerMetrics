namespace KafkaConsumerMetrics
{
    public record OffsetData(string Topic, long TopicCurrentOffset, string ConsumerGroupId, int Partition, long ConsumerOffset);
}
