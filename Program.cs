using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Application.Services;
using Appointment_Scheduling_System.ConsoleUI.Menus;
using Appointment_Scheduling_System.Infrastructure.Data;
using Appointment_Scheduling_System.Infrastructure.Persistence;
using Appointment_Scheduling_System.Infrastructure.Repositories;

namespace Appointment_Scheduling_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileService = new JsonFileService();
            var context = new JsonDataContext(fileService);

            // Repositories
            IClientRepository clientRepo = new ClientRepository(context);
            IAppointmentRepository appointmentRepo = new AppointmentRepository(context);
            IServiceRepository serviceRepo = new ServiceRepository(context);
            IScheduleRepository scheduleRepo = new ScheduleRepository(context);
            IStaffRepository staffRepo = new StaffRepository(context);
            var staffService = new StaffService(staffRepo);

            // Services
            var clientService = new ClientService(clientRepo);
            var appointmentService = new AppointmentService(appointmentRepo, scheduleRepo);
            var reportService = new ReportService(appointmentRepo, serviceRepo);

            // UI
            var mainMenu = new MainMenu(
              clientService,
              serviceRepo,
              appointmentService,
              reportService,
              staffService,
              scheduleRepo
            );

            mainMenu.Show();
        }
    }
}