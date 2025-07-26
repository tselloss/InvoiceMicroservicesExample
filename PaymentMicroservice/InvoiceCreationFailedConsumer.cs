using MassTransit;
using static MessageContracts.MessageContract;

namespace PaymentMicroservice
{
    public class InvoiceCreationFailedConsumer : IConsumer<IInvoiceCreationFailed>
    {
        public Task Consume(ConsumeContext<IInvoiceCreationFailed> context)
        {
            Console.WriteLine($"[ERROR] Invoice creation failed for customer {context.Message.CustomerNumber}");
            Console.WriteLine($"Reason: {context.Message.Error} at {context.Message.Timestamp}");

            return Task.CompletedTask;
        }
    }
}
