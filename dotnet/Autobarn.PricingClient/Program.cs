using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Autobarn.PricingClient {
    class Program {
        static void Main(string[] args) {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostBuilderContext, services) => {
                    var amqp = hostBuilderContext.Configuration.GetConnectionString("RabbitMQ");
                    var bus = RabbitHutch.CreateBus(amqp);
                    services.AddSingleton(bus);
                    var grpcServerUrl = hostBuilderContext.Configuration["GrpcServerUrl"];
                    var channel = GrpcChannel.ForAddress(grpcServerUrl);
                    services.AddSingleton(channel);
                    services.AddHostedService<PricingClientService>();
                });
            var host = builder.Build();
            host.Run();
        }
    }
}
