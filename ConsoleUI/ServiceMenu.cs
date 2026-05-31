using Appointment_Scheduling_System.Application.Services;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ServiceMenu
    {
        private readonly ServiceManagementService _serviceService;

        public ServiceMenu(ServiceManagementService serviceService)
        {
            _serviceService = serviceService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== Services ===");
                Console.WriteLine("1. Add Service");
                Console.WriteLine("2. List Services");
                Console.WriteLine("0. Back");

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
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Duration (minutes): ");
            int duration = int.Parse(Console.ReadLine());

            _serviceService.AddService(name, duration, price);

            Console.WriteLine("Service added.");
            Console.ReadKey();
        }

        private void ListServices()
        {
            var services = _serviceService.GetAllServices();

            foreach (var service in services)
            {
                Console.WriteLine(
                    $"{service.Id} | {service.Name} | {service.Price} lv | {service.DurationInMinutes} min");
            }

            Console.ReadKey();
        }
    }
}