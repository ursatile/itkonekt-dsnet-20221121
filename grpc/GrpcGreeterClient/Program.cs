using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeter;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
using var channel = GrpcChannel.ForAddress("https://localhost:7275");
var client = new Greeter.GreeterClient(channel);
Console.WriteLine("Press any key to send a gRPC request!");
Console.WriteLine("1: English GB");
Console.WriteLine("2: English US");
Console.WriteLine("3: English AU");

while(true) {    
    var key = Console.ReadKey(true);
    var languageCode = key.KeyChar switch {
        '2' => "en-US",
        '3' => "en-AU",
        _ => "en-GB"
    };
    var request = new HelloRequest {
        LanguageCode = languageCode,
        Name = "ITKonekt!"
    };
    var reply = await client.SayHelloAsync(request);
    Console.WriteLine(reply);
}
