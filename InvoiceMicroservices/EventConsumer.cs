using MassTransit;
using static MessageContracts.MessageContract;

namespace InvoiceMicroservices
{
    public class EventConsumer : IConsumer<IInvoiceToCreate>
    {
        public async Task Consume(ConsumeContext<IInvoiceToCreate> context)
        {
            try
            {
                var newInvoiceNumber = new Random().Next(10000, 99999);

                Console.WriteLine($"Creating invoice {newInvoiceNumber} for customer: {context.Message.CustomerNumber}");

                context.Message.InvoiceItems.ForEach(i =>
                {
                    Console.WriteLine($"With items: Price: {i.Price}, Desc: {i.Description}");
                    Console.WriteLine($"Actual distance in miles: {i.ActualMileage}, Base Rate: {i.BaseRate}");
                    Console.WriteLine($"Oversized: {i.IsOversized}, Refrigerated: {i.IsRefrigerated}, Haz Mat: {i.IsHazardousMaterial}");
                });

                if (!context.Message.InvoiceItems.Any()) throw new Exception("No invoice items provided.");

                await context.Publish<IInvoiceCreated>(new
                {
                    InvoiceNumber = newInvoiceNumber,
                    InvoiceData = new
                    {
                        context.Message.CustomerNumber,
                        context.Message.InvoiceItems
                    }
                });
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error creating invoice for customer {context.Message.CustomerNumber}: {ex.Message}";
                Console.WriteLine(errorMessage);

                await context.Publish<IInvoiceCreationFailed>(new
                {
                    context.Message.CustomerNumber,
                    Error = errorMessage,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
