using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeter;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
using var channel = GrpcChannel.ForAddress("https://localhost:7275");
var client = new Greeter.GreeterClient(channel);
Console.WriteLine("Press any key to send a gRPC request!");
while(true) {
    Console.ReadKey();
    var request = new HelloRequest {
        Name = "ITKonekt!"
    };
    var reply = await client.SayHelloAsync(request);
    Console.WriteLine(reply);
}
