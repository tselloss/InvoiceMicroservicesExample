using MessageContracts;

namespace TestClient
{
    public static class InvoiceItemsMock
    {
        private static readonly Random _rnd = new();

        public static List<InvoiceItems> GetRandomListOrNull()
        {
            bool returnNull = _rnd.Next(0, 2) == 0;

            if (returnNull)
                return new List<InvoiceItems>(); 

            return new List<InvoiceItems>
            {
                new InvoiceItems
                {
                    Description = "Tables",
                    Price = Math.Round(_rnd.NextDouble() * 100, 2),
                    ActualMileage = 40,
                    BaseRate = 12.50,
                    IsHazardousMaterial = false,
                    IsOversized = true,
                    IsRefrigerated = false
                },
                new InvoiceItems
                {
                    Description = "Chairs",
                    Price = Math.Round(_rnd.NextDouble() * 100, 2),
                    ActualMileage = 40,
                    BaseRate = 12.50,
                    IsHazardousMaterial = false,
                    IsOversized = false,
                    IsRefrigerated = false
                }
            };
        }
    }
}
