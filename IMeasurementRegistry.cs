namespace KafkaConsumerMetrics
{
    public interface IMeasurementRegistry
    {
        void RegisterOffset(OffsetData offsetData);
    }
}