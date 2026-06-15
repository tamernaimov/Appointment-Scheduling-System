using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.ConsoleUI.Menus;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class MainMenu
    {
        private readonly ClientMenu _clientMenu;
        private readonly ServiceMenu _serviceMenu;
        private readonly StaffMenu _staffMenu;
        private readonly AppointmentMenu _appointmentMenu;
        private readonly ReportMenu _reportMenu;
        private readonly ScheduleMenu _scheduleMenu;

        public MainMenu(
            ClientMenu clientMenu,
            ServiceMenu serviceMenu,
            StaffMenu staffMenu,
            AppointmentMenu appointmentMenu,
            ReportMenu reportMenu,
            ScheduleMenu scheduleMenu)
        {
            _clientMenu = clientMenu;
            _serviceMenu = serviceMenu;
            _staffMenu = staffMenu;
            _appointmentMenu = appointmentMenu;
            _reportMenu = reportMenu;
            _scheduleMenu = scheduleMenu;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("========================================");
                Console.WriteLine("   APPOINTMENT SCHEDULING SYSTEM");
                Console.WriteLine("========================================");

                Console.WriteLine("[1] Clients");
                Console.WriteLine("[2] Services");
                Console.WriteLine("[3] Staff");
                Console.WriteLine("[4] Appointments");
                Console.WriteLine("[5] Reports");
                Console.WriteLine("[6] Schedule");
                Console.WriteLine("[0] Exit");

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _clientMenu.Show();
                        break;

                    case "2":
                        _serviceMenu.Show();
                        break;

                    case "3":
                        _staffMenu.Show();
                        break;

                    case "4":
                        _appointmentMenu.Show();
                        break;

                    case "5":
                        _reportMenu.Show();
                        break;

                    case "6":
                        _scheduleMenu.Show();
                        break;

                    case "0":
                        return;
                }
            }
        }
    }
}