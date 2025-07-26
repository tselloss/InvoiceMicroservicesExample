// See https://aka.ms/new-console-template for more information
using InvoiceMicroservices;
using MassTransit;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("localhost", 5672, "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
    cfg.ReceiveEndpoint("invoice-service", e =>
    {
        e.UseInMemoryOutbox();
        e.Consumer<EventConsumer>(c =>
          c.UseMessageRetry(m => m.Interval(5, new TimeSpan(0, 0, 10))));
    });
});
var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);

Console.WriteLine("Invoice microservice now listening");

try
{
    while (true)
    {
        //sit in while loop listening for messages
        await Task.Delay(100);
    }
}
finally
{
    await busControl.StopAsync();
}


