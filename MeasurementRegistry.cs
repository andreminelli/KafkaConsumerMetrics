using System.Diagnostics.Metrics;

namespace KafkaConsumerMetrics
{
    internal class MeasurementRegistry : IMeasurementRegistry
    {
        static readonly ObservableGauge<long> consumerLag = MeterSource.Meter.CreateObservableGauge<long>("consumer-lag", GetMeasurements);
        static readonly Dictionary<string, Measurement<long>> data = new();

        public void RegisterOffset(OffsetData offsetData)
        {
            Console.WriteLine($"Consumer {offsetData.ConsumerGroupId} (in topic {offsetData.Topic}) lag in partition {offsetData.Partition}: {offsetData.TopicCurrentOffset - offsetData.ConsumerOffset}");

            data[$"{offsetData.Topic}-{offsetData.ConsumerGroupId}-{offsetData.Partition}"] = new Measurement<long>(
                offsetData.TopicCurrentOffset - offsetData.ConsumerOffset,
                new("topic", offsetData.Topic),
                new("partition", offsetData.Partition),
                new("consumerGroup", offsetData.ConsumerGroupId)
                );
        }

        private static IEnumerable<Measurement<long>> GetMeasurements()
        {
            return data.Values.ToArray();
        }
    }
}
