namespace ClientInvoiceManager
{
    public class InvoiceItem
    {
        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // NEW 05/31/2026:
        // Needed so JSON can reload InvoiceItem objects from file.
        public InvoiceItem()
        {
        }

        public InvoiceItem(string description, decimal quantity, decimal unitPrice)
        {
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public decimal GetLineTotal()
        {
            return Quantity * UnitPrice;
        }
    }
}