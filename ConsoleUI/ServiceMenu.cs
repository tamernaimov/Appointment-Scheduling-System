using Appointment_Scheduling_System.Application.Interfaces;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ServiceMenu
    {
        private readonly IServiceManagementService _serviceService;

        public ServiceMenu(IServiceManagementService serviceService)
        {
            _serviceService = serviceService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Header("SERVICES");

                Console.WriteLine("[1] Add Service");
                Console.WriteLine("[2] List Services");
                Console.WriteLine("[0] Back");

                Console.Write("\nSelect: ");

                switch (Console.ReadLine())
                {
                    case "1": Add(); break;
                    case "2": List(); break;
                    case "0": return;
                }
            }
        }

        private void Add()
        {
            Console.Clear();
            Header("ADD SERVICE");

            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price))
            {
                Error("Invalid price");
                return;
            }

            Console.Write("Duration: ");
            if (!int.TryParse(Console.ReadLine(), out var dur))
            {
                Error("Invalid duration");
                return;
            }

            _serviceService.AddService(name, dur, price);
            Success("Service added");
        }

        private void List()
        {
            Console.Clear();
            Header("SERVICE LIST");

            foreach (var s in _serviceService.GetAllServices())
                Console.WriteLine($"{s.Id} | {s.Name} | {s.Price} | {s.DurationInMinutes}");

            Pause();
        }

        private void Header(string t)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("==================================");
            Console.WriteLine($"        {t}");
            Console.WriteLine("==================================");
            Console.ResetColor();
        }

        private void Success(string m)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {m}");
            Console.ResetColor();
            Pause();
        }

        private void Error(string m)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {m}");
            Console.ResetColor();
            Pause();
        }

        private void Pause() => Console.ReadKey();
    }
}