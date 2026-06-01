using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace ClientInvoiceManager
{
    public class InvoiceManager
    {
        private List<Client> clients = new List<Client>();

        private List<Invoice> invoices = new List<Invoice>();

        private int nextClientId = 1001;

        private int nextInvoiceId = 5001;

        // NEW 05/31/2026:
        // JSON files used to save and load client and invoice records.
        private string clientFilePath = "clients.json";

        private string invoiceFilePath = "invoices.json";

        // NEW 05/31/2026:
        // Constructor runs automatically when InvoiceManager is created in Program.cs.
        // It loads old records before the user begins using the menu.
        public InvoiceManager()
        {
            LoadDataFromFiles();
        }

        public void AddClient()
        {
            // UPDATED 06/01/2026 1:37 PM:
            // Updated client examples to match a technology services business.
            Console.WriteLine("\n=== Add New Client ===");
            Console.WriteLine("Add a client who receives technology services.");
            Console.WriteLine("Example client: Desert Dental Group");
            Console.WriteLine("Example email: admin@desertdental.com");
            Console.WriteLine("Example phone: 480-555-0199\n");

            Console.Write("Client name: ");
            string clientName = Console.ReadLine();

            Console.Write("Client email: ");
            string email = Console.ReadLine();

            Console.Write("Client phone: ");
            string phone = Console.ReadLine();

            Client client = new Client(nextClientId, clientName, email, phone);

            clients.Add(client);

            // NEW 05/31/2026:
            // Saves client records after a new client is added.
            SaveDataToFiles();

            Console.WriteLine($"\nClient added successfully. Client ID: {nextClientId}");

            nextClientId++;
        }

        public void ViewClients()
        {
            Console.WriteLine("\n=== Client List ===");

            if (clients.Count == 0)
            {
                Console.WriteLine("No clients have been added yet.");
                return;
            }

            foreach (Client client in clients)
            {
                Console.WriteLine($"\nClient ID: {client.ClientId}");
                Console.WriteLine($"Name: {client.ClientName}");
                Console.WriteLine($"Email: {client.Email}");
                Console.WriteLine($"Phone: {client.Phone}");
            }
        }

        public void CreateInvoice()
        {
            Console.WriteLine("\n=== Create Invoice ===");

            if (clients.Count == 0)
            {
                Console.WriteLine("You must add a client before creating an invoice.");
                return;
            }

            DisplayClientSummary();

            Console.Write("\nEnter Client ID for this invoice: ");
            string input = Console.ReadLine().Trim();

            bool isValidNumber = int.TryParse(input, out int clientId);

            if (!isValidNumber)
            {
                Console.WriteLine("Invalid input. Please enter a numeric Client ID.");
                return;
            }

            Client selectedClient = clients.Find(client => client.ClientId == clientId);

            if (selectedClient == null)
            {
                Console.WriteLine("No client with that ID was found.");
                return;
            }

            Invoice invoice = new Invoice(nextInvoiceId, clientId);

            bool addingItems = true;

            while (addingItems)
            {
                // UPDATED 06/01/2026 1:37 PM:
                // Reworked invoice item examples for managed IT, software,
                // and cybersecurity service billing.
                Console.WriteLine("\n=== Add Technology Service Item ===");
                Console.WriteLine("Each invoice item represents ONE service being billed to the client.");
                Console.WriteLine("The program calculates each line item like this:");
                Console.WriteLine();
                Console.WriteLine("Quantity x Unit Price = Line Total");
                Console.WriteLine();
                Console.WriteLine("Technology service examples:");
                Console.WriteLine("- Network security assessment | Quantity: 4 hours | Unit Price: 150.00");
                Console.WriteLine("- Managed IT support          | Quantity: 1 month | Unit Price: 500.00");
                Console.WriteLine("- Software bug fix            | Quantity: 2 hours | Unit Price: 125.00");
                Console.WriteLine("- Workstation setup           | Quantity: 3 devices | Unit Price: 85.00");
                Console.WriteLine();
                Console.WriteLine("For quantity, enter the number of hours, months, devices, or service units.");
                Console.WriteLine("For unit price, enter the price for one hour, one month, one device, or one service.");
                Console.WriteLine();

                Console.Write("Service description, such as Network security assessment: ");
                string description = Console.ReadLine();

                decimal quantity = GetDecimalFromUser("Quantity, such as 1, 2, 2.5, or 4: ");

                decimal unitPrice = GetDecimalFromUser("Unit price, such as 85, 125.00, or 500: ");

                

                InvoiceItem item = new InvoiceItem(description, quantity, unitPrice);

                invoice.AddItem(item);

                Console.Write("Add another item? Enter y or n: ");
                string choice = Console.ReadLine().ToLower();

                if (choice != "y")
                {
                    addingItems = false;
                }
            }

            invoices.Add(invoice);

            // NEW 05/31/2026:
            // Saves invoice records after a new invoice is created.
            SaveDataToFiles();

            Console.WriteLine($"\nInvoice created successfully. Invoice ID: {nextInvoiceId}");
            Console.WriteLine($"Client: {selectedClient.ClientName}");
            Console.WriteLine($"Invoice Total: {invoice.GetInvoiceTotal():C}");

            nextInvoiceId++;
        }

        public void ViewInvoices()
        {
            Console.WriteLine("\n=== All Invoices ===");

            if (invoices.Count == 0)
            {
                Console.WriteLine("No invoices have been created yet.");
                return;
            }

            foreach (Invoice invoice in invoices)
            {
                DisplayInvoice(invoice);
            }
        }

        // NEW 05/31/2026:
        // Displays only unpaid invoices so the user can focus on money still owed.
        public void ViewUnpaidInvoices()
        {
            Console.WriteLine("\n=== Unpaid Invoices ===");

            bool found = false;

            foreach (Invoice invoice in invoices)
            {
                if (!invoice.IsPaid)
                {
                    DisplayInvoice(invoice);
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No unpaid invoices found.");
            }
        }

        // NEW 05/31/2026:
        // Displays only paid invoices so the user can review completed payments.
        public void ViewPaidInvoices()
        {
            Console.WriteLine("\n=== Paid Invoices ===");

            bool found = false;

            foreach (Invoice invoice in invoices)
            {
                if (invoice.IsPaid)
                {
                    DisplayInvoice(invoice);
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No paid invoices found.");
            }
        }

        public void MarkInvoicePaid()
        {
            Console.WriteLine("\n=== Mark Invoice Paid ===");

            if (invoices.Count == 0)
            {
                Console.WriteLine("No invoices available.");
                return;
            }

            DisplayInvoiceSummary();

            Console.Write("\nEnter Invoice ID to mark paid: ");
            string input = Console.ReadLine().Trim();

            bool isValidNumber = int.TryParse(input, out int invoiceId);

            if (!isValidNumber)
            {
                Console.WriteLine("Invalid input. Please enter a numeric Invoice ID.");
                return;
            }

            Invoice invoice = invoices.Find(inv => inv.InvoiceId == invoiceId);

            if (invoice == null)
            {
                Console.WriteLine("No invoice with that ID was found.");
                return;
            }

            invoice.IsPaid = true;

            // NEW 05/31/2026:
            invoice.DatePaid = DateTime.Now;

            // NEW 05/31/2026:
            // Saves the paid status update.
            SaveDataToFiles();

            Console.WriteLine($"Invoice {invoice.InvoiceId} marked as paid.");
        }

        // UPDATED 05/31/2026:
        // Provides a clearer business-style revenue summary with invoice counts,
        // client count, paid totals, unpaid totals, and collection status.
        public void ViewRevenueSummary()
        {
            // UPDATED 06/01/2026 1:37 PM:
            // Rebranded the summary as a technology services revenue report.
            Console.WriteLine("\n================================");
            Console.WriteLine(" TECHNOLOGY SERVICES REVENUE");
            Console.WriteLine("================================");

            if (invoices.Count == 0)
            {
                Console.WriteLine("No invoices have been created yet.");
                Console.WriteLine("Create invoices first to generate revenue summary data.");
                return;
            }

            decimal totalBilled = 0;
            decimal totalPaid = 0;
            decimal totalUnpaid = 0;

            int paidInvoiceCount = 0;
            int unpaidInvoiceCount = 0;

            foreach (Invoice invoice in invoices)
            {
                decimal invoiceTotal = invoice.GetInvoiceTotal();

                totalBilled += invoiceTotal;

                if (invoice.IsPaid)
                {
                    totalPaid += invoiceTotal;
                    paidInvoiceCount++;
                }
                else
                {
                    totalUnpaid += invoiceTotal;
                    unpaidInvoiceCount++;
                }
            }

            decimal collectionRate = totalBilled > 0
                ? (totalPaid / totalBilled) * 100
                : 0;

            Console.WriteLine($"Clients in System: {clients.Count}");
            Console.WriteLine($"Total Invoices: {invoices.Count}");
            Console.WriteLine($"Paid Invoices: {paidInvoiceCount}");
            Console.WriteLine($"Unpaid Invoices: {unpaidInvoiceCount}");
            Console.WriteLine();

            Console.WriteLine("--- Financial Totals ---");
            Console.WriteLine($"Total Billed: {totalBilled:C}");
            Console.WriteLine($"Total Paid: {totalPaid:C}");
            Console.WriteLine($"Total Still Owed: {totalUnpaid:C}");
            Console.WriteLine($"Collection Rate: {collectionRate:F1}%");
            Console.WriteLine();

            if (unpaidInvoiceCount == 0)
            {
                Console.WriteLine("Status: All invoices are currently paid.");
            }
            else if (collectionRate >= 75)
            {
                Console.WriteLine("Status: Most billed revenue has been collected.");
            }
            else if (collectionRate >= 50)
            {
                Console.WriteLine("Status: About half or more of billed revenue has been collected.");
            }
            else
            {
                Console.WriteLine("Status: A large amount of billed revenue is still unpaid.");
            }
        }


        private decimal GetDecimalFromUser(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string input = Console.ReadLine().Trim();

                bool isValidDecimal = decimal.TryParse(input, out decimal value);

                if (isValidDecimal && value >=0)
                { 
                    return value;
                }

                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
        }

        private void DisplayClientSummary()
        {
            Console.WriteLine("\n--- Current Clients ---");

            foreach (Client client in clients)
            {
                Console.WriteLine($"ID: {client.ClientId} | {client.ClientName}");
            }
        }

        private void DisplayInvoiceSummary()
        {
            Console.WriteLine("\n--- Current Invoices ---");

            foreach (Invoice invoice in invoices)
            {
                string status = invoice.IsPaid ? "Paid" : "Unpaid";

                Console.WriteLine($"Invoice ID: {invoice.InvoiceId} | Client ID: {invoice.ClientId} | Total: {invoice.GetInvoiceTotal():C} | Status: {status}");

            }
        }

        // UPDATED 05/31/2026:
        // Improved invoice display so users understand the difference between
        // item line totals and the full invoice total.
        private void DisplayInvoice(Invoice invoice)
        {
            Client client = clients.Find(c => c.ClientId == invoice.ClientId);

            string clientName = client == null ? "Unknown Client" : client.ClientName;

            string status = invoice.IsPaid ? "Paid" : "Unpaid";

            Console.WriteLine("\n------------------------------");
            Console.WriteLine($"Invoice ID: {invoice.InvoiceId}");
            Console.WriteLine($"Client: {clientName}");
            Console.WriteLine($"Date Created: {invoice.DateCreated}");
            Console.WriteLine($"Status: {status}");

            if (invoice.DatePaid.HasValue)
            {
                Console.WriteLine($"Date Paid: {invoice.DatePaid.Value}");
            }

            Console.WriteLine("\nInvoice Items:");
            Console.WriteLine("Each item below is calculated as: Quantity x Unit Price = Line Total");
            Console.WriteLine();

            foreach (InvoiceItem item in invoice.Items)
            {
                Console.WriteLine($"Item: {item.Description}");
                Console.WriteLine($"Quantity: {item.Quantity}");
                Console.WriteLine($"Unit Price: {item.UnitPrice:C}");
                Console.WriteLine($"Line Total: {item.GetLineTotal():C}");
                Console.WriteLine();
            }

            Console.WriteLine("Invoice Total:");
            Console.WriteLine("This is the combined total of all line items on this invoice.");
            Console.WriteLine($"Total Amount Due: {invoice.GetInvoiceTotal():C}");
        }


        // NEW 05/31/2026:
        // Saves both clients and invoices to JSON files.
        private void SaveDataToFiles()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string clientJson = JsonSerializer.Serialize(clients, options);
            string invoiceJson = JsonSerializer.Serialize(invoices, options);

            File.WriteAllText(clientFilePath, clientJson);
            File.WriteAllText(invoiceFilePath, invoiceJson);
        }

        // NEW 05/31/2026:
        // Loads clients and invoices from JSON files when the program starts.
        private void LoadDataFromFiles()
        {
            if (File.Exists(clientFilePath))
            {
                string clientJson = File.ReadAllText(clientFilePath);

                if (!string.IsNullOrWhiteSpace(clientJson))
                {
                    clients = JsonSerializer.Deserialize<List<Client>>(clientJson);

                    if (clients == null)
                    {
                        clients = new List<Client>();
                    }
                }
            }

            if (File.Exists(invoiceFilePath))
            {
                string invoiceJson = File.ReadAllText(invoiceFilePath);

                if (!string.IsNullOrWhiteSpace(invoiceJson))
                {
                    invoices = JsonSerializer.Deserialize<List<Invoice>>(invoiceJson);

                    if (invoices == null)
                    {
                        invoices = new List<Invoice>();
                    }
                }
            }

            // NEW 05/31/2026:
            // After loading old records, update the next available IDs
            // so new clients and invoices do not reuse old IDs.
            foreach (Client client in clients)
            {
                if (client.ClientId >= nextClientId)
                {
                    nextClientId = client.ClientId + 1;
                }
            }

            foreach (Invoice invoice in invoices)
            {
                if (invoice.InvoiceId >= nextInvoiceId)
                {
                    nextInvoiceId = invoice.InvoiceId + 1;
                }

                // NEW 05/31/2026:
                // Safety check in case an older invoice loads without an item list.
                if (invoice.Items == null)
                {
                    invoice.Items = new List<InvoiceItem>();
                }
            }
        }

    }
}