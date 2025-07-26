using MassTransit;
using static MessageContracts.MessageContract;

namespace PaymentMicroservice
{
    public class InvoiceCreatedConsumer : IConsumer<IInvoiceCreated>
    {
        public Task Consume(ConsumeContext<IInvoiceCreated> context)
        {
            Console.WriteLine($"Received message for invoice number: {context.Message.InvoiceNumber}");

            return Task.CompletedTask;
        }
    }
}
