namespace ClientInvoiceManager
{
    public class Client
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        // NEW 05/31/2026:
        // Needed so JSON can reload Client objects from file.
        public Client()
        {
        }

        public Client(int clientId, string clientName, string email, string phone)
        {
            ClientId = clientId;
            ClientName = clientName;
            Email = email;
            Phone = phone;
        }
    }
}