using Appointment_Scheduling_System.Application.Services;
using Appointment_Scheduling_System.Domain.Entities;
using Azure;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ClientMenu
    {
        private readonly IClientService _clientService;

        public ClientMenu(IClientService clientService)
        {
            _clientService = clientService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("==================================================");
                Console.WriteLine("                    CLIENTS");
                Console.WriteLine("==================================================");

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine("[1] Add Client");
                Console.WriteLine("[2] Edit Client");
                Console.WriteLine("[3] List Clients"); //list clients

                Console.WriteLine();
                Console.WriteLine("[0] Back");

                Console.WriteLine();
                Console.Write("Choose option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Add();
                        break;

                    case "2":
                        Edit();
                        break;

                    case "3":
                        List();
                        break;

                    case "0":
                        return;
                }
            }
        }
        void Add()
        {
            Console.Write("First name: ");
            var fn = Console.ReadLine();

            Console.Write("Last name: ");
            var ln = Console.ReadLine();

            Console.Write("Phone: ");
            var ph = Console.ReadLine();

            Console.Write("Email: ");
            var em = Console.ReadLine();

            _clientService.CreateClient(fn, ln, ph, em);
        }

        void Edit()
        {

            //not sure
            List(false);

            Console.Write("Id: ");
            int id = int.Parse(Console.ReadLine());

            var client = _clientService.GetClient(id);

            if (client == null)
            {
                Console.WriteLine("Client not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("First name: ");
            var first = Console.ReadLine();

            Console.Write("Last name: ");
            var last = Console.ReadLine();

            Console.Write("Phone: ");
            var phone = Console.ReadLine();

            Console.Write("Email: ");
            var email = Console.ReadLine();

            try
            {
                client.SetName(first, last);
                client.SetContact(phone, email);

                _clientService.UpdateClient(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            _clientService.UpdateClient(client);

            Console.WriteLine("Client updated.");
            Console.ReadKey();
        }

        void List(bool pause = true)
        {
            foreach (var c in _clientService.GetAllClients())
            {
                Console.WriteLine($"{c.Id} {c.FirstName} {c.LastName}");
            }

            if (pause)
                Console.ReadKey();
        }
    }
}