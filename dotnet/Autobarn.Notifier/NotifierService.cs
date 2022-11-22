using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.Notifier {
    public class NotifierService : IHostedService {
        private readonly string subscriptionId = $"Autobarn.Notifier@{Environment.MachineName}";
        private readonly IBus bus;
        private readonly ILogger<NotifierService> logger;

        public NotifierService(ILogger<NotifierService> logger, IBus bus) {
            this.logger = logger;
            this.bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            Console.WriteLine("Starting NotifierService...");
            logger.LogInformation("Starting NotifierService...");
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(
                subscriptionId, HandleNewVehiclePriceMessage);
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            logger.LogInformation("Stopping NotifierService...");
            Console.WriteLine("Stopping NotifierService...");
            return Task.CompletedTask;
        }

        private void HandleNewVehiclePriceMessage(NewVehicleMessage message) {
            logger.LogInformation("Handling NewVehiclePriceMessage");
            logger.LogInformation(message.ToString());
        }
    }
}
