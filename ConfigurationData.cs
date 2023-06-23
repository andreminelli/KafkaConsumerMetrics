namespace KafkaConsumerMetrics
{
    public class ConfigurationData
    {
        public int QueryIntervalSecs { get; init; } = 30;
        public List<KafkaAccessInformation>? Clusters { get; init; }
    }
}
