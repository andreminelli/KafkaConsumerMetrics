using System.Diagnostics.Metrics;

namespace KafkaConsumerMetrics
{
    internal class MeasurementRegistry : IMeasurementRegistry
    {
        static readonly ObservableGauge<long> consumerLag = MeterSource.Meter.CreateObservableGauge<long>("consumer-lag", GetMeasurements);

        public void RegisterOffset(string topic, long topicCurrentOffset, string consumerGroupId, int partition, long consumerOffset)
        {
            Console.WriteLine($"Consumer {consumerGroupId} (in topic {topic}) lag in partition {partition}: {topicCurrentOffset - consumerOffset}");
        }

        private static IEnumerable<Measurement<long>> GetMeasurements()
        {
            throw new NotImplementedException();
        }
    }
}
