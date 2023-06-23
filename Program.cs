using Confluent.Kafka;
using KafkaConsumerMetrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args).Build();

var configuration = host.Services.GetRequiredService<IConfiguration>();
var configurationData = configuration.GetSection("Settings").Get<ConfigurationData>();

foreach (var cluster in configurationData.Clusters)
{
    var kafkaConfig = cluster.Config;
    var adminClient = new AdminClientBuilder(kafkaConfig).Build();
    var consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig(kafkaConfig) { GroupId = "Monitor" }).Build();

    var metaData = adminClient.GetMetadata(TimeSpan.FromSeconds(5));

    var groups = await adminClient.ListConsumerGroupsAsync();

    var info = await adminClient.DescribeConsumerGroupsAsync(groups.Valid.Select(g => g.GroupId));

    foreach (var group in groups.Valid)
    {
        var consumerOffsets = await adminClient.ListConsumerGroupOffsetsAsync(new[] { new ConsumerGroupTopicPartitions(group.GroupId, null) });
        foreach (var groupPartitions in consumerOffsets.SelectMany(o => o.Partitions))
        {
            //var currentOffsets = consumer.PositionTopicPartitionOffset(groupPartitions.TopicPartition);
            var waterMarkOffset = consumer.QueryWatermarkOffsets(groupPartitions.TopicPartition, TimeSpan.FromSeconds(1));
            Console.WriteLine($"Consumer {group.GroupId} (in topic {groupPartitions.Topic}) lag in partition {groupPartitions.Partition.Value}: {waterMarkOffset.High - groupPartitions.Offset}");
        }
    }

}
