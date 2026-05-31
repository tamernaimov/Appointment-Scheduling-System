using Appointment_Scheduling_System.Application.Services;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class AppointmentMenu
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentMenu(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public void Show()
        {
            Console.WriteLine("\n--- Appointments ---");
            Console.WriteLine("1. Create appointment");
            Console.WriteLine("2. List appointments");

            var choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.Write("ClientId: ");
                int clientId = int.Parse(Console.ReadLine());

                Console.Write("StaffId: ");
                int staffId = int.Parse(Console.ReadLine());

                Console.Write("ServiceId: ");
                int serviceId = int.Parse(Console.ReadLine());

                Console.Write("Start time (yyyy-MM-dd HH:mm): ");
                DateTime start = DateTime.Parse(Console.ReadLine());

                Console.Write("End time (yyyy-MM-dd HH:mm): ");
                DateTime end = DateTime.Parse(Console.ReadLine());

                try
                {
                    _appointmentService.CreateAppointment(new Appointment
                    {
                        ClientId = clientId,
                        StaffId = staffId,
                        ServiceId = serviceId,
                        StartTime = start,
                        EndTime = end
                    });

                    Console.WriteLine("Appointment created!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else if (choice == "2")
            {
                var appointments = _appointmentService.GetAll();

                foreach (var a in appointments)
                {
                    Console.WriteLine($"{a.Id}: Client {a.ClientId}, {a.StartTime}");
                }
            }
        }
    }
}