using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.Pricing;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingClient {
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

        private async Task CalculateVehiclePrice(NewVehicleMessage message) {
            logger.LogInformation($"Calculating price for {message}...");
            var pr = new PriceRequest {
                Color = message.Color,
                Manufacturer = message.ManufacturerName,
                Year = message.Year,
                Model = message.ModelName
            };
            var client = new Pricer.PricerClient(channel);
            var priceReply = await client.GetPriceAsync(pr);
            logger.LogInformation($"Calculated price: {priceReply.Price} {priceReply.CurrencyCode}");
            var newVehiclePriceMessage = message.AddPrice(priceReply.Price, priceReply.CurrencyCode);
            await bus.PubSub.PublishAsync(newVehiclePriceMessage);
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
    }
}
