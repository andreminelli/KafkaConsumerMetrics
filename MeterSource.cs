using System.Diagnostics.Metrics;

namespace KafkaConsumerMetrics
{
    internal static class MeterSource
    {
        internal static Meter Meter = new Meter("Contrib.KafkaConsumerMetrics");
    }
}
