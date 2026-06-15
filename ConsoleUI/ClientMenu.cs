using Appointment_Scheduling_System.Application.Interfaces;

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
                Console.WriteLine("[1] Add Client");
                Console.WriteLine("[2] Edit Client");
                Console.WriteLine("[3] List Clients");
                Console.WriteLine("[0] Back");

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
            Console.Write("First: ");
            var fn = Console.ReadLine();

            Console.Write("Last: ");
            var ln = Console.ReadLine();

           

            Console.Write("Phone: ");
            var ph = Console.ReadLine();

            Console.Write("Email: ");
            var em = Console.ReadLine();


            if (string.IsNullOrWhiteSpace(fn) ||
                string.IsNullOrWhiteSpace(ln) ||
                string.IsNullOrWhiteSpace(ph) ||
                string.IsNullOrWhiteSpace(em))
            {
                Console.WriteLine("Invalid input.");
                Console.ReadKey();
                return;
            }
            _clientService.CreateClient(fn, ln, ph, em);
        }

        void Edit()
        {
            List(false);

            Console.Write("Id: ");
            int id = int.Parse(Console.ReadLine());

            var client = _clientService.GetClient(id);

            if (client == null)
                return;

            Console.Write("First: ");
            var f = Console.ReadLine();

            Console.Write("Last: ");
            var l = Console.ReadLine();

            Console.Write("Phone: ");
            var p = Console.ReadLine();

            Console.Write("Email: ");
            var e = Console.ReadLine();

            client.SetName(f, l);
            client.SetContact(p, e);

            _clientService.UpdateClient(client);
        }

        void List(bool pause = true)
        {
            foreach (var c in _clientService.GetAllClients())
                Console.WriteLine($"{c.Id} {c.FirstName} {c.LastName}");

            if (pause)
                Console.ReadKey();
        }
    }
}