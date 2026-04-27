using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Application.Services;
using Appointment_Scheduling_System.ConsoleUI.Menus;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Data;
using Appointment_Scheduling_System.Infrastructure.Persistence;
using Appointment_Scheduling_System.Infrastructure.Repositories;

namespace Appointment_Scheduling_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // setup
            var fileService = new JsonFileService();
            var context = new JsonDataContext(fileService);

            IClientRepository clientRepo = new ClientRepository(context);
            IAppointmentRepository appointmentRepo = new AppointmentRepository(context);

            var clientService = new ClientService(clientRepo);
            var appointmentService = new AppointmentService(appointmentRepo);

            // start app
            var menu = new MainMenu(clientService, appointmentService);
            menu.Show();
        }
    }
}
