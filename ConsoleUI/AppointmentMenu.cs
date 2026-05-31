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
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== APPOINTMENTS ===");
                Console.WriteLine("1. Create Appointment");
                Console.WriteLine("2. Cancel Appointment");
                Console.WriteLine("3. Complete Appointment");
                Console.WriteLine("4. Mark NoShow");
                Console.WriteLine("5. List All");
                Console.WriteLine("0. Back");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Create();
                        break;

                    case "2":
                        Cancel();
                        break;

                    case "3":
                        Complete();
                        break;

                    case "4":
                        NoShow();
                        break;

                    case "5":
                        List();
                        break;

                    case "0":
                        return;
                }
            }
        }
        private void Create()
        {
            try
            {
                Console.Write("Client Id: ");
                int clientId = int.Parse(Console.ReadLine());

                Console.Write("Staff Id: ");
                int staffId = int.Parse(Console.ReadLine());

                Console.Write("Service Id: ");
                int serviceId = int.Parse(Console.ReadLine());

                Console.Write("Start (yyyy-mm-dd hh:mm): ");
                DateTime start = DateTime.Parse(Console.ReadLine());

                Console.Write("End (yyyy-mm-dd hh:mm): ");
                DateTime end = DateTime.Parse(Console.ReadLine());

                var appointment = new Appointment
                {
                    ClientId = clientId,
                    StaffId = staffId,
                    ServiceId = serviceId,
                    StartTime = start,
                    EndTime = end
                };

                _appointmentService.CreateAppointment(appointment);

                Console.WriteLine("Appointment created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        private void Cancel()
        {
            Console.Write("Appointment Id: ");
            int id = int.Parse(Console.ReadLine());

            try
            {
                _appointmentService.CancelAppointment(id);
                Console.WriteLine("Cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
        private void Complete()
        {
            Console.Write("Appointment Id: ");
            int id = int.Parse(Console.ReadLine());

            _appointmentService.CompleteAppointment(id);

            Console.WriteLine("Completed.");
            Console.ReadKey();
        }

        private void NoShow()
        {
            Console.Write("Appointment Id: ");
            int id = int.Parse(Console.ReadLine());

            _appointmentService.MarkAsNoShow(id);

            Console.WriteLine("Marked as NoShow.");
            Console.ReadKey();
        }

        private void List()
        {
            var list = _appointmentService.GetAll();

            foreach (var a in list)
            {
                Console.WriteLine(
                    $"{a.Id} | C:{a.ClientId} S:{a.StaffId} " +
                    $"Service:{a.ServiceId} | {a.StartTime} - {a.EndTime} | {a.Status}"
                );
            }

            Console.ReadKey();
        }
    }
}