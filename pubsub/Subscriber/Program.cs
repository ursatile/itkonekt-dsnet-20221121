using Messages;
using EasyNetQ;

const string AMQP = "amqps://eehnnebc:1r-uJfqEaU6bf6LJ1j-aMwLsusZvKbXK@hasty-purple-aardvark.rmq4.cloudamqp.com/eehnnebc";
var bus = RabbitHutch.CreateBus(AMQP);

bus.PubSub.Subscribe<Greeting>(Environment.MachineName, HandleGreeting);
Console.WriteLine("Subscribed to <Greeting> messages...");
Console.ReadLine();



static void HandleGreeting(Greeting greeting) {
    if (greeting.Number % 5 == 0) {
        throw new Exception("We can't handle messages whose number is divisible by five!");
    }    
    Console.WriteLine(greeting);
}
