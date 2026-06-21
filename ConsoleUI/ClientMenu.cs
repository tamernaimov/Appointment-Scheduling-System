using Appointment_Scheduling_System.Application.Interfaces;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ClientMenu : MenuBase
    {
        protected override ConsoleColor AccentColor => ConsoleColor.Yellow;

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
                Header("CLIENTS");
                Console.WriteLine("[1] Add Client");
                Console.WriteLine("[2] Edit Client");
                Console.WriteLine("[3] List Clients");
                Console.WriteLine("[0] Back");
                Console.Write("\nSelect: ");
                switch (Console.ReadLine())
                {
                    case "1": Add(); break;
                    case "2": Edit(); break;
                    case "3": List(); break;
                    case "0": return;
                }
            }
        }

        private void Add()
        {
            Console.Clear();
            Header("ADD CLIENT");
            Console.Write("First name: ");
            var fn = Console.ReadLine();
            Console.Write("Last name: ");
            var ln = Console.ReadLine();
            Console.Write("Phone: ");
            var ph = Console.ReadLine();
            Console.Write("Email: ");
            var em = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fn) || string.IsNullOrWhiteSpace(ln))
            {
                Error("Invalid input");
                return;
            }

            TryRun(() => _clientService.CreateClient(fn, ln, ph, em), "Client created");
        }

        private void Edit()
        {
            List(false);
            Console.Write("\nClient Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            var c = _clientService.GetClient(id);
            if (c == null)
            {
                Error("Not found");
                return;
            }

            Console.Write("First: ");
            var fn = Console.ReadLine();
            Console.Write("Last: ");
            var ln = Console.ReadLine();
            Console.Write("Phone: ");
            var ph = Console.ReadLine();
            Console.Write("Email: ");
            var em = Console.ReadLine();

            TryRun(() =>
            {
                c.SetName(fn, ln);
                c.SetContact(ph, em);
                _clientService.UpdateClient(c);
            }, "Updated");
        }

        private void List(bool pause = true)
        {
            Console.Clear();
            Header("CLIENT LIST");

            PrintTable(
                new[] { "Id", "First Name", "Last Name" },
                _clientService.GetAllClients().Select(c => new[] { c.Id.ToString(), c.FirstName, c.LastName }));

            if (pause) Pause();
        }
    }
}