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
                Console.WriteLine("[4] Delete Client");
                Console.WriteLine("[0] Back");
                Console.Write("\nSelect: ");
                switch (Console.ReadLine())
                {
                    case "1": Add(); break;
                    case "2": Edit(); break;
                    case "3": List(); break;
                    case "4": Delete(); break;
                    case "0": return;
                }
            }
        }

        private void Add()
        {
            Console.Clear();
            Header("ADD CLIENT");

            var fn = ReadRequired("First name");
            var ln = ReadRequired("Last name");
            var ph = ReadRequired("Phone");
            var em = ReadString("Email", v => !string.IsNullOrWhiteSpace(v) && v.Contains('@'), "Невалиден имейл формат.");

            TryRun(() => _clientService.CreateClient(fn, ln, ph, em), "Client created");
        }

        private void Edit()
        {
            List(false);
            var clients = _clientService.GetAllClients();

            int id = ReadInt("\nClient Id", cid => clients.Any(x => x.Id == cid), "Няма клиент с такъв Id.");
            var c = clients.First(x => x.Id == id);

            var fn = ReadRequired("First");
            var ln = ReadRequired("Last");
            var ph = ReadRequired("Phone");
            var em = ReadString("Email", v => !string.IsNullOrWhiteSpace(v) && v.Contains('@'), "Невалиден имейл формат.");

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

        private void Delete()
        {
            List(false);
            var clients = _clientService.GetAllClients();
            if (clients.Count == 0)
            {
                Pause();
                return;
            }

            int id = ReadInt("\nClient Id", cid => clients.Any(x => x.Id == cid), "Няма клиент с такъв Id.");
            var c = clients.First(x => x.Id == id);

            if (!Confirm($"\nИзтрий {c.FirstName} {c.LastName}?"))
                return;

            TryRun(() => _clientService.DeleteClient(id), "Client deleted");
        }
    }
}