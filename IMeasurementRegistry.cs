namespace KafkaConsumerMetrics
{
    public interface IMeasurementRegistry
    {
        void RegisterOffset(string topic, long topicCurrentOffset, string consumerGroupId, int partition, long consumerOffset);
    }
}