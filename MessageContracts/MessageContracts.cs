namespace MessageContracts
{
    public class MessageContracts
    {
        public interface IInvoiceCreated
        { 
            int InvoiceNumber { get;}
            IInvoiceCreated InvoiceData { get; }
        }

        public interface IInvoiceToCreate
        { 
            int CustomerNumber { get; set; }
            List<InvoiceItems> InvoiceItems { get; set; }
        }
    }
}