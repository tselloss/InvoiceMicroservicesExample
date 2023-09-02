using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MessageContracts.MessageContracts;

namespace TestClient
{
    public class InvoiceCreatedConsumer : IConsumer<IInvoiceCreated>
    {
        public async Task Consume(ConsumeContext<IInvoiceCreated> context)
        {
            await Task.Run(() => Console.WriteLine($"Invoice with number: {context.Message.InvoiceNumber} was created."));
        }
    }
}
