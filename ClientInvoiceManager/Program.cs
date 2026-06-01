/* Jeff O'Hara
 * 5/31/2026
 * 
 * This app is an Invoice Manager app intended for small businesses to record and track invoice entries and hold accounts stored in JSON files
 * for use in read/write and storage. There is a clear UI that allows for the adding of a client, viewing clients, creating invoices, viewing all
 * invoices, viewing paid/unpaid invoices, marking invoices paid and having a revenue summary table to show how well the business is doing and to 
 * keep track of all outstanding invoices.
 */


using System;

namespace ClientInvoiceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            InvoiceManager manager = new InvoiceManager();

            bool running = true;

            while (running)
            {
                // UPDATED 06/01/2026 1:37 PM:
                // Rebranded the app from a generic invoice manager
                // into a technology services billing system.
                Console.WriteLine("\n==========================================");
                Console.WriteLine("   TECHNOLOGY SERVICES BILLING SYSTEM");
                Console.WriteLine("==========================================");
                Console.WriteLine("Track clients, service invoices, payments,");
                Console.WriteLine("and revenue for a technology consulting or");
                Console.WriteLine("managed IT services business.");
                Console.WriteLine();
                Console.WriteLine("1. Add client");
                Console.WriteLine("2. View clients");
                Console.WriteLine("3. Create invoice");
                Console.WriteLine("4. View all invoices");
                Console.WriteLine("5. View unpaid invoices");
                Console.WriteLine("6. View paid invoices");
                Console.WriteLine("7. Mark invoice paid");
                Console.WriteLine("8. View revenue summary");
                Console.WriteLine("9. Exit");
                Console.Write("\nChoose an option: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    manager.AddClient();
                }
                else if (choice == "2")
                {
                    manager.ViewClients();
                }
                else if (choice == "3")
                {
                    manager.CreateInvoice();
                }
                else if (choice == "4")
                {
                    manager.ViewInvoices();
                }
                else if (choice == "5")
                {
                    manager.ViewUnpaidInvoices();
                }
                else if (choice == "6")
                {
                    manager.ViewPaidInvoices();
                }
                else if (choice == "7")
                {
                    manager.MarkInvoicePaid();
                }
                else if (choice == "8")
                {
                    manager.ViewRevenueSummary();
                }
                else if (choice == "9")
                {
                    running = false;
                    Console.WriteLine("Exiting Client Invoice Manager.");
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose 1 through 9.");
                }
            }
        }
    }
}