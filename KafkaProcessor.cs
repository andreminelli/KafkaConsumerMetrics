using Confluent.Kafka;

namespace KafkaConsumerMetrics
{
    public class KafkaProcessor
    {
        private readonly IMeasurementRegistry _measurementRegistry;

        public KafkaProcessor(IMeasurementRegistry measurementRegistry)
        {
            _measurementRegistry = measurementRegistry;
        }

        public async Task ProcessAsync(KafkaAccessInformation accessInformation)
        {
            var kafkaConfig = accessInformation.Config;

            using var adminClient = new AdminClientBuilder(kafkaConfig).Build();
            using var consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig(kafkaConfig) { GroupId = "Monitor" }).Build();

            var metaData = adminClient.GetMetadata(TimeSpan.FromSeconds(5));

            var groups = await adminClient.ListConsumerGroupsAsync();

            var groupDescriptions = await adminClient.DescribeConsumerGroupsAsync(groups.Valid.Select(g => g.GroupId));

            foreach (var group in groups.Valid)
            {
                var consumerOffsets = await adminClient.ListConsumerGroupOffsetsAsync(new[] { new ConsumerGroupTopicPartitions(group.GroupId, null) });
                foreach (var groupPartitions in consumerOffsets.SelectMany(o => o.Partitions))
                {
                    var waterMarkOffset = consumer.QueryWatermarkOffsets(groupPartitions.TopicPartition, TimeSpan.FromSeconds(1));

                    var data = new OffsetData(accessInformation.Name!, groupPartitions.Topic, waterMarkOffset.High.Value, group.GroupId, groupPartitions.Partition.Value, groupPartitions.Offset.Value);
                    _measurementRegistry.RegisterOffset(data);
                }
            }
        }
    }
}
