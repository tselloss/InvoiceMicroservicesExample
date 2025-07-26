namespace MessageContracts
{
    public class MessageContract
    {
        public interface IInvoiceCreated
        {
            int InvoiceNumber { get; }
            IInvoiceCreated InvoiceData { get; }
        }

        public interface IInvoiceToCreate
        {
            int CustomerNumber { get; set; }
            List<InvoiceItems> InvoiceItems { get; set; }
        }

        public interface IInvoiceCreationFailed
        {
            int CustomerNumber { get; }
            string Error { get; }
            DateTime Timestamp { get; }
        }
    }
}