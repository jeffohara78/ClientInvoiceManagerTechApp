using System;
using System.Collections.Generic;

namespace ClientInvoiceManager
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public int ClientId { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsPaid { get; set; }

        public List<InvoiceItem> Items { get; set; }

        // NEW 05/31/2026:
        // Needed so JSON can reload Invoice objects from file.
        public Invoice()
        {
            Items = new List<InvoiceItem>();
        }

        // NEW 05/31/2026:
        // Stores the date and time when the invoice was marked as paid.
        // Nullable DateTime means it can be empty until the invoice is paid.
        public DateTime? DatePaid { get; set; }

        public Invoice(int invoiceId, int clientId)
        {
            InvoiceId = invoiceId;
            ClientId = clientId;
            DateCreated = DateTime.Now;
            IsPaid = false;

            // NEW 5/31/2026
            // New invoices are unpaid, so there is no paid date yet
            DatePaid = null;

            Items = new List<InvoiceItem>();
        }

        public void AddItem(InvoiceItem item)
        {
            Items.Add(item);
        }

        public decimal GetInvoiceTotal()
        {
            decimal total = 0;

            foreach (InvoiceItem item in Items)
            {
                total += item.GetLineTotal();
            }

            return total;
        }
    }
}