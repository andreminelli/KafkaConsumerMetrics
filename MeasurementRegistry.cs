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
				new("cluster-name", offsetData.ClusterName),
				new("cluster-consumerGroup", offsetData.ConsumerGroupId),
				new("topic-name", offsetData.Topic),
                new("topic-partition", offsetData.Partition)
                );
        }

        private static IEnumerable<Measurement<long>> GetMeasurements()
        {
            return data.Values.ToArray();
        }
    }
}
