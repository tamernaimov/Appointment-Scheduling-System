using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ServiceMenu
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceMenu(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== SERVICES ===");
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

            Console.Write("Duration (min): ");
            int duration = int.Parse(Console.ReadLine());

            _serviceRepository.Add(new Service
            {
                Name = name,
                Price = price,
                DurationInMinutes = duration
            });

            Console.WriteLine("Service added.");
            Console.ReadKey();
        }
        private void ListServices()
        {
            var services = _serviceRepository.GetAll();

            foreach (var s in services)
            {
                Console.WriteLine($"{s.Id} | {s.Name} | {s.Price} | {s.DurationInMinutes} min");
            }

            Console.ReadKey();
        }
    }
}