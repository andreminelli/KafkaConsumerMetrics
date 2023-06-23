using System.Diagnostics.Metrics;

namespace KafkaConsumerMetrics
{
    internal static class MeterSource
    {
        internal const string Name = "Contrib.KafkaConsumerMetrics";
        internal static Meter Meter = new Meter(Name);
    }
}
