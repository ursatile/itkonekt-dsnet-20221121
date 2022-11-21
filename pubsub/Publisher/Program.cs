using Messages;
using EasyNetQ;

const string AMQP = "amqps://eehnnebc:1r-uJfqEaU6bf6LJ1j-aMwLsusZvKbXK@hasty-purple-aardvark.rmq4.cloudamqp.com/eehnnebc";

var bus = RabbitHutch.CreateBus(AMQP);
var count = 0;

Console.WriteLine("Press any key to publish a message...");
while(true) {
    Console.ReadKey(true);
    var message = new Greeting {
        Message = $"This is message {count++}"
    };
    bus.PubSub.Publish(message);
    Console.WriteLine("Published a message:");
    Console.WriteLine(message);
}