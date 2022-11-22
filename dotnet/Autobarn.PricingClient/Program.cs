using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingClient {
    class Program {
        static void Main(string[] args) {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostBuilderContext, services) => {
                    var amqp = hostBuilderContext.Configuration.GetConnectionString("RabbitMQ");
                    var bus = RabbitHutch.CreateBus(amqp);
                    services.AddSingleton(bus);
                    var channel = GrpcChannel.ForAddress("https://workshop.ursatile.com:5003/");
                    services.AddSingleton(channel);
                    services.AddHostedService<PricingClientService>();
                })
                .ConfigureLogging((_, logging) => {
                    logging.ClearProviders();
                    logging.AddSimpleConsole(options => options.IncludeScopes = true);
                    // logging.AddEventLog();
                });

            var host = builder.Build();
            host.Run();
        }
    }

    public class PricingClientService : IHostedService {

        private readonly string subscriptionId = $"Autobarn.PricingClient@{Environment.MachineName}";

        private readonly ILogger<PricingClientService> logger;
        private readonly IBus bus;
        private readonly GrpcChannel channel;

        public PricingClientService(ILogger<PricingClientService> logger, IBus bus,
            GrpcChannel channel) {
            this.logger = logger;
            this.bus = bus;
            this.channel = channel;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(subscriptionId, CalculateVehiclePrice);
        }

        private void CalculateVehiclePrice(NewVehicleMessage message) {
            logger.LogInformation($"Calculating price for {message}...");
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
    }
}
