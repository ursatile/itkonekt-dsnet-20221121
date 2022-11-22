using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Autobarn.Notifier {
    public class NotifierService : IHostedService {
        private readonly string subscriptionId = $"Autobarn.Notifier@{Environment.MachineName}";
        private readonly IBus bus;
        private readonly HubConnection hub;
        private readonly ILogger<NotifierService> logger;

        public NotifierService(ILogger<NotifierService> logger, IBus bus, HubConnection hub) {
            this.logger = logger;
            this.bus = bus;
            this.hub = hub;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            logger.LogInformation("Starting NotifierService...");
            await hub.StartAsync();
            logger.LogInformation("Started SignalR Hub!");
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(
                subscriptionId, HandleNewVehiclePriceMessage);
        }

        public async Task StopAsync(CancellationToken cancellationToken) {
            logger.LogInformation("Stopping NotifierService...");
            await hub.StopAsync();
        }

        private async Task HandleNewVehiclePriceMessage(NewVehicleMessage message) {
            logger.LogInformation("Handling NewVehiclePriceMessage");
            logger.LogInformation(message.ToString());
            var json = JsonConvert.SerializeObject(message);
            logger.LogDebug(json);
            await hub.SendAsync("NotifyAllThePeopleWhoAreOnOurWebsite",
                "Autobarn.Notifier",
                json);
            logger.LogInformation("Sent message to the SignalR Hub.");
        }
    }
}
