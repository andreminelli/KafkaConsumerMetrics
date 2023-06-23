using KafkaConsumerMetrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry;

using IHost host = Host.CreateDefaultBuilder(args).Build();

var configuration = host.Services.GetRequiredService<IConfiguration>();
var configurationData = configuration.GetSection("Settings").Get<ConfigurationData>() ?? throw new NullReferenceException("Missing 'Settings' section on json configuration ");

// TODO Add more validations over ConfigurationData/KafkaAccessInformation

using MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
              .AddMeter(MeterSource.Name)
              .AddPrometheusHttpListener(options =>
              {
                  options.UriPrefixes = new[] { $"http://localhost:{configurationData.PrometheusPort}/" };
              })
              .Build() ?? throw new NullReferenceException("Can't configure metric exporting");

var registy = new MeasurementRegistry();

foreach (var kafkaAccessInformation in configurationData.Clusters)
{
    await new KafkaProcessor(registy).ProcessAsync(kafkaAccessInformation);
}

Console.ReadKey();
