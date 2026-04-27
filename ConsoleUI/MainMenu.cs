using Appointment_Scheduling_System.Application.Services;
using static System.Net.Mime.MediaTypeNames;


namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class MainMenu
    {
        private readonly ClientService _clientService;
        private readonly AppointmentService _appointmentService;

        public MainMenu(ClientService clientService, AppointmentService appointmentService)
        {
            _clientService = clientService;
            _appointmentService = appointmentService;
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Clients");
                Console.WriteLine("2. Appointments");
                Console.WriteLine("0. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        new ClientMenu(_clientService).Show();
                        break;

                    case "2":
                        new AppointmentMenu(_appointmentService).Show();
                        break;

                    case "0":
                        return;
                }
            }
        }
    }
}
