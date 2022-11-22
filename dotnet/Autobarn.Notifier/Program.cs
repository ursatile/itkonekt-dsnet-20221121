using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Autobarn.Notifier {
    class Program {
        static void Main(string[] args) {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostBuilderContext, services) => {
                    var amqp = hostBuilderContext.Configuration.GetConnectionString("RabbitMQ");
                    var bus = RabbitHutch.CreateBus(amqp);
                    services.AddSingleton(bus);
                    services.AddHostedService<NotifierService>();
                });

            var host = builder.Build();
            host.Run();
        }
    }
}
