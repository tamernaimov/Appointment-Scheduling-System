using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

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

                Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine("==================================================");
                Console.WriteLine("                   SERVICES");
                Console.WriteLine("==================================================");

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine("[1] Add Service");
                Console.WriteLine("[2] List Services");

                Console.WriteLine();
                Console.WriteLine("[0] Back");

                Console.WriteLine();
                Console.Write("Choose option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddService();
                        break;

                    case "2":
                        ListServices();
                        break;

                    case "0":
                        return;
                }
            }
        }

        private void AddService()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid price.");
                Console.ReadKey();
                return;
            }

            Console.Write("Duration (min): ");
            if (!int.TryParse(Console.ReadLine(), out int duration))
            {
                Console.WriteLine("Invalid duration.");
                Console.ReadKey();
                return;
            }

            _serviceService.AddService(name, duration, price);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Service added successfully!");
            Console.ResetColor();

            Console.ReadKey();
        }
        private void ListServices()
        {
            var services = _serviceService.GetAllServices();

            foreach (var s in services)
            {
                Console.WriteLine($"{s.Id} | {s.Name} | {s.Price} | {s.DurationInMinutes}");
            }

            Console.ReadKey();
        }
    }
}