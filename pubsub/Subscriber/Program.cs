using Messages;
using EasyNetQ;

const string AMQP = "amqps://eehnnebc:1r-uJfqEaU6bf6LJ1j-aMwLsusZvKbXK@hasty-purple-aardvark.rmq4.cloudamqp.com/eehnnebc";
var bus = RabbitHutch.CreateBus(AMQP);

bus.PubSub.Subscribe<Greeting>("SUBSCRIPTION_ID", greeting => {
    Console.WriteLine(greeting);
});
Console.WriteLine("Subscribed to <Greeting> messages...");
Console.ReadLine();