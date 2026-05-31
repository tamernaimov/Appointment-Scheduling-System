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

            IClientRepository clientRepo = new ClientRepository(context);
            IAppointmentRepository appointmentRepo = new AppointmentRepository(context);
            IServiceRepository serviceRepo = new ServiceRepository(context);
            IStaffRepository staffRepo = new StaffRepository(context);

            var clientService = new ClientService(clientRepo);
            var appointmentService = new AppointmentService(appointmentRepo);
            var serviceService = new ServiceManagementService(serviceRepo);
            var staffService = new StaffService(staffRepo);

            var mainMenu = new MainMenu(
                clientService,
                appointmentService,
                serviceService,
                staffService);

            mainMenu.Show();
        }
    }
}