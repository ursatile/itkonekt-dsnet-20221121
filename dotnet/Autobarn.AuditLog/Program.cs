using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Autobarn.AuditLog {
    class Program {
        static void Main(string[] args) {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostBuilderContext, services) => {
                    var amqp = hostBuilderContext.Configuration.GetConnectionString("RabbitMQ");
                    var bus = RabbitHutch.CreateBus(amqp);
                    services.AddSingleton(bus);
                }).Build();
            host.Run();
        }
    }
}
