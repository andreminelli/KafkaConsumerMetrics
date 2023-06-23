namespace KafkaConsumerMetrics
{
    public class ConfigurationData
    {
        public int QueryIntervalSecs { get; init; } = 30;
        public int PrometheusPort { get; init; } = 9000;
        public List<KafkaAccessInformation>? Clusters { get; init; }
    }
}
