# KafkaConsumerMetrics

Inspired on [Kafka Lag Exporter](https://github.com/seglo/kafka-lag-exporter), this is a .Net implementation of a Kafka consumer metrics exporter, using `System.Diagnostics.Metrics` to collect metrics and `Prometheus` to expose them.

TODO:

- [ ] Export max lag (over all partitions for each consumer)
- [ ] Export current offsets
- [ ] Create integration tests
- [ ] Publish container image
- [ ] Create a time-based lag metric (to indicate latency, using interpolation or another similar technique)
