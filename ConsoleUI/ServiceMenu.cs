using Appointment_Scheduling_System.Application.Interfaces;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ServiceMenu : MenuBase
    {
        protected override ConsoleColor AccentColor => ConsoleColor.Magenta;

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

            var name = ReadRequired("Name");
            var price = ReadDecimal("Price", p => p >= 0, "Цената не може да е отрицателна.");
            var dur = ReadInt("Duration", d => d > 0, "Продължителността трябва да е положителна.");

            TryRun(() => _serviceService.AddService(name, dur, price), "Service added");
        }

        private void List()
        {
            Console.Clear();
            Header("SERVICE LIST");

            PrintTable(
                new[] { "Id", "Name", "Price", "Duration" },
                _serviceService.GetAllServices().Select(s =>
                    new[] { s.Id.ToString(), s.Name, s.Price.ToString(), s.DurationInMinutes.ToString() }));

            Pause();
        }
    }
}