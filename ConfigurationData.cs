namespace KafkaConsumerMetrics
{
    public class ConfigurationData
    {
        public int QueryIntervalSecs { get; set; }
        public List<KafkaAccessInformation> Clusters { get; set; }
    }
}
