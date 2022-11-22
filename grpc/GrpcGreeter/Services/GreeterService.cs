using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        Console.WriteLine(request.LanguageCode);
        var message = request.LanguageCode switch {
            "en-GB" => $"Good morning, {request.Name}.",
            "en-US" => $"Howdy, {request.Name}.",
            "en-AU" => $"G'day, {request.Name}.",
            _ => "Hello {request.Name}"
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
