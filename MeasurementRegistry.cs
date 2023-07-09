using System.Diagnostics.Metrics;

namespace KafkaConsumerMetrics
{
	internal class MeasurementRegistry : IMeasurementRegistry
    {
		static readonly ObservableGauge<long> consumerLag = MeterSource.Meter.CreateObservableGauge<long>("consumer-lag", GetLagMeasurements);
		static readonly Dictionary<string, Measurement<long>> lagMeasurements = new();

		static readonly ObservableCounter<long> consumerOffset = MeterSource.Meter.CreateObservableCounter<long>("consumer-offset", GetOffsetMeasurements);
		static readonly Dictionary<string, Measurement<long>> offsetMeasurements = new();

		public void RegisterOffset(OffsetData offsetData)
        {
            Console.WriteLine($"Consumer {offsetData.ConsumerGroupId} (in topic {offsetData.Topic}) lag in partition {offsetData.Partition}: {offsetData.TopicCurrentOffset - offsetData.ConsumerOffset}");

			lagMeasurements[$"{offsetData.Topic}-{offsetData.ConsumerGroupId}-{offsetData.Partition}"] = new Measurement<long>(
				offsetData.TopicCurrentOffset - offsetData.ConsumerOffset,
				BuildTags(offsetData)
				);

			offsetMeasurements[$"{offsetData.Topic}-{offsetData.ConsumerGroupId}-{offsetData.Partition}"] = new Measurement<long>(
				offsetData.ConsumerOffset,
				BuildTags(offsetData)
				);
		}

		private static IEnumerable<KeyValuePair<string, object?>> BuildTags(OffsetData offsetData)
		{
			yield return new("cluster-name", offsetData.ClusterName);
			yield return new("cluster-consumerGroup", offsetData.ConsumerGroupId);
			yield return new("topic-name", offsetData.Topic);
			yield return new("topic-partition", offsetData.Partition);
		}

		private static IEnumerable<Measurement<long>> GetLagMeasurements()
		{
			return lagMeasurements.Values.ToArray();
		}

		private static IEnumerable<Measurement<long>> GetOffsetMeasurements()
		{
			return offsetMeasurements.Values.ToArray();
		}
	}
}
