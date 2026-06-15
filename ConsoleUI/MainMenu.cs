using Appointment_Scheduling_System.Application.Services;
using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class MainMenu
    {
       
        private readonly IServiceProvider _provider;

        public MainMenu(IServiceProvider provider)
        {
            _provider = provider;
        }
        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("MAIN MENU");
                Console.WriteLine("[1] Clients");
                Console.WriteLine("[2] Services");
                Console.WriteLine("[3] Staff");
                Console.WriteLine("[4] Appointments");
                Console.WriteLine("[5] Reports");
                Console.WriteLine("[6] Schedule");
                Console.WriteLine("[0] Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _provider.GetRequiredService<ClientMenu>().Show();
                        break;

                    case "2":
                        _provider.GetRequiredService<ServiceMenu>().Show();
                        break;

                    case "3":
                        _provider.GetRequiredService<StaffMenu>().Show();
                        break;

                    case "4":
                        _provider.GetRequiredService<AppointmentMenu>().Show();
                        break;

                    case "5":
                        _provider.GetRequiredService<ReportMenu>().Show();
                        break;

                    case "6":
                        _provider.GetRequiredService<ScheduleMenu>().Show();
                        break;

                    case "0":
                        return;
                }
            }
        }
    }
}