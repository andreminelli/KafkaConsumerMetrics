using System.Diagnostics.Metrics;

namespace KafkaConsumerMetrics
{
    internal class MeasurementRegistry : IMeasurementRegistry
    {
        static readonly ObservableGauge<long> consumerLag = MeterSource.Meter.CreateObservableGauge<long>("consumer-lag", GetMeasurements);
        static readonly Dictionary<string, Measurement<long>> data = new();

        public void RegisterOffset(string topic, long topicCurrentOffset, string consumerGroupId, int partition, long consumerOffset)
        {
            Console.WriteLine($"Consumer {consumerGroupId} (in topic {topic}) lag in partition {partition}: {topicCurrentOffset - consumerOffset}");
            data[$"{topic}-{consumerGroupId}-{partition}"] = new Measurement<long>(
                topicCurrentOffset - consumerOffset,
                new ("topic", topic),
                new ("partition", partition),
                new("consumerGroup", consumerGroupId)
                );
        }

        private static IEnumerable<Measurement<long>> GetMeasurements()
        {
            return data.Values.ToArray();
        }
    }
}
