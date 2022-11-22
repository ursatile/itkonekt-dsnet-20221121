using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        var name = request.FirstName + " " + request.LastName;
        Console.WriteLine(request.LanguageCode);
        var message = request.LanguageCode switch {
            "en-GB" => $"Good morning, {name}.",
            "en-US" => $"Howdy, {name}.",
            "en-AU" => $"G'day, {name}.",
            _ => "Hello {name}"
        };
        message += request.Friendliness switch {
            -1 => " You look terrible.",
            1 => " You look lovely.",
            _ => ""
        };
        return Task.FromResult(new HelloReply {
            Message = message
        });
    }
}
