using KafkaConsumerMetrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args).Build();

var config = host.Services.GetRequiredService<IConfiguration>();
var configurationData = config.GetSection("Settings").Get<ConfigurationData>();

Console.WriteLine("Hello, World!");
