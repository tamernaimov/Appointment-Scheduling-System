using Appointment_Scheduling_System.Application.Services;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class MainMenu
    {
        private readonly ClientService _clientService;
        private readonly AppointmentService _appointmentService;
        private readonly ServiceManagementService _serviceService;
        private readonly StaffService _staffService;

        public MainMenu(
            ClientService clientService,
            AppointmentService appointmentService,
            ServiceManagementService serviceService,
            StaffService staffService)
        {
            _clientService = clientService;
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _staffService = staffService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("==================================");
                Console.WriteLine(" Appointment Scheduling System");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Clients");
                Console.WriteLine("2. Services");
                Console.WriteLine("3. Staff");
                Console.WriteLine("4. Appointments");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==================================");

                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Client menu not implemented yet.");
                        Console.ReadKey();
                        break;

                    case "2":
                        new ServiceMenu(_serviceService).Show();
                        break;

                    case "3":
                        new StaffMenu(_staffService).Show();
                        break;

                    case "4":
                        Console.WriteLine("Appointment menu not implemented yet.");
                        Console.ReadKey();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}