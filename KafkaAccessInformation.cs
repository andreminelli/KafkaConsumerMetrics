using Confluent.Kafka;

namespace KafkaConsumerMetrics
{
    public class KafkaAccessInformation
    {
        public string? Name { get; init; }
        public ClientConfig? Config { get; init; }
    }
}
