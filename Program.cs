using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Application.Services;
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
            var fileService = new JsonFileService();
            var context = new JsonDataContext(fileService);

            IClientRepository clientRepo = new ClientRepository(context);
            IAppointmentRepository appointmentRepo = new AppointmentRepository(context);

            var clientService = new ClientService(clientRepo);
            var appointmentService = new AppointmentService(appointmentRepo);


            clientService.CreateClient("Ivan", "Ivanov", "0888", "ivan@test.com");
            clientService.CreateClient("Petar", "Petrov", "0899", "petar@test.com");

            Console.WriteLine("Clients:");
            foreach (var c in clientService.GetAllClients())
            {
                Console.WriteLine($"{c.Id}: {c.FirstName} {c.LastName}");
            }

            var appointment = new Appointment
            {
                ClientId = 1,
                StaffId = 1,
                ServiceId = 1,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };

            appointmentService.CreateAppointment(appointment);

            Console.WriteLine("\nAppointments:");
            foreach (var a in appointmentService.GetAll())
            {
                Console.WriteLine($"Id: {a.Id}, Client: {a.ClientId}, Time: {a.StartTime}");
            }

            try
            {
                var conflictAppointment = new Appointment
                {
                    ClientId = 2,
                    StaffId = 1,
                    ServiceId = 1,
                    StartTime = appointment.StartTime, // същото време
                    EndTime = appointment.EndTime
                };

                appointmentService.CreateAppointment(conflictAppointment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nExpected error: {ex.Message}");
            }

            appointmentService.CancelAppointment(1);

            Console.WriteLine("\nAfter cancel:");
            foreach (var a in appointmentService.GetAll())
            {
                Console.WriteLine($"Id: {a.Id}, Status: {a.Status}");
            }
        }
    }
}
