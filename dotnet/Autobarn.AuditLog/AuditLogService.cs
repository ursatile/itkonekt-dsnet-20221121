using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.AuditLog {
    public class AuditLogService : IHostedService {
        private readonly string subscriptionId = $"Autobarn.AuditLog@{Environment.MachineName}";
        private readonly IBus bus;
        private readonly ILogger<AuditLogService> logger;

        public AuditLogService(ILogger<AuditLogService> logger, IBus bus) {
            this.logger = logger;
            this.bus = bus;
        }


        public async Task StartAsync(CancellationToken cancellationToken) {
            logger.LogInformation("Starting AuditLogService...");
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(
                subscriptionId, HandleNewVehicleMessage);
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            logger.LogInformation("Stopping AuditLogService...");
            return Task.CompletedTask;
        }

        private void HandleNewVehicleMessage(NewVehicleMessage message) {
            logger.LogInformation("Handling NewVehicleMessage");
            logger.LogInformation(message.ToString());
        }
    }
}
