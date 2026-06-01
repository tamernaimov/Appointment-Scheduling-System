using Appointment_Scheduling_System.Application.Services;
using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Infrastructure.Repositories;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class MainMenu
    {
        private readonly ClientService _clientService;
        private readonly IServiceRepository _serviceRepository;
        private readonly AppointmentService _appointmentService;
        private readonly ReportService _reportService;
        private readonly StaffService _staffService;
        private readonly IScheduleRepository _scheduleRepository;
        public MainMenu(
            ClientService clientService,
            IServiceRepository serviceRepository,
            AppointmentService appointmentService,
            ReportService reportService,
            IScheduleRepository scheduleRepository)
        {
            _clientService = clientService;
            _serviceRepository = serviceRepository;
            _appointmentService = appointmentService;
            _reportService = reportService;
            _scheduleRepository = scheduleRepository;
        }
        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== SYSTEM MENU ===");
                Console.WriteLine("1. Clients");
                Console.WriteLine("2. Services");
                Console.WriteLine("3. Staff");
                Console.WriteLine("4. Appointments");
                Console.WriteLine("5. Reports");
                Console.WriteLine("0. Exit");

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        new ClientMenu(_clientService).Show();
                        break;

                    case "2":
                        new ServiceMenu(_serviceRepository).Show();
                        break;
                    case "3":
                        new StaffMenu(_staffService).Show();
                        break;

                    case "4":
                        new AppointmentMenu(_appointmentService).Show();
                        break;

                    case "5":
                        new ReportMenu(_reportService).Show();
                        break;

                    case "6":
                        new ScheduleMenu(_scheduleRepository).Show();
                        break;
                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}