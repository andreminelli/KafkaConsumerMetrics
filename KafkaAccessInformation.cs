using Confluent.Kafka;

namespace KafkaConsumerMetrics
{
    public class KafkaAccessInformation
    {
        public string Name { get; set; }
        public ClientConfig Config { get; set; }
    }
}
