using Appointment_Scheduling_System.Application.Services;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ClientMenu
    {
        private readonly ClientService _clientService;

        public ClientMenu(ClientService clientService)
        {
            _clientService = clientService;
        }

        public void Show()
        {
            Console.WriteLine("\n--- Clients ---");
            Console.WriteLine("1. Add client");
            Console.WriteLine("2. List clients");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("First name: ");
                var firstName = Console.ReadLine();

                Console.Write("Last name: ");
                var lastName = Console.ReadLine();

                Console.Write("Phone: ");
                var phone = Console.ReadLine();

                Console.Write("Email: ");
                var email = Console.ReadLine();

                _clientService.CreateClient(firstName, lastName, phone, email);
            }
            else if (choice == "2")
            {
                var clients = _clientService.GetAllClients();

                foreach (var c in clients)
                {
                    Console.WriteLine($"{c.Id}: {c.FirstName} {c.LastName}");
                }
            }
        }
    }
}