using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Application.Services;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class AppointmentMenu
    {
        private readonly AppointmentService _appointmentService;
        private readonly ClientService _clientService;
        private readonly StaffService _staffService;
        private readonly IServiceRepository _serviceRepository;

        public AppointmentMenu(
            AppointmentService appointmentService,
            ClientService clientService,
            StaffService staffService,
            IServiceRepository serviceRepository)
        {
            _appointmentService = appointmentService;
            _clientService = clientService;
            _staffService = staffService;
            _serviceRepository = serviceRepository;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("==================================================");
                Console.WriteLine("                 APPOINTMENTS");
                Console.WriteLine("==================================================");

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine("[1] Create Appointment");
                Console.WriteLine("[2] Edit Appointment");
                Console.WriteLine("[3] Cancel Appointment");
                Console.WriteLine("[4] Complete Appointment");
                Console.WriteLine("[5] Mark NoShow");
                Console.WriteLine("[6] List All");

                Console.WriteLine();
                Console.WriteLine("[0] Back");

                Console.WriteLine();
                Console.Write("Choose option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Create();
                        break;

                    case "2":
                        Edit();
                        break;

                    case "3":
                        Cancel();
                        break;

                    case "4":
                        Complete();
                        break;

                    case "5":
                        NoShow();
                        break;

                    case "6":
                        ListAll();
                        break;

                    case "0":
                        return;
                }
            }
        }

        private void Create()
        {
            Console.Clear();

            Console.WriteLine("=== CLIENTS ===");

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-25}");
            Console.WriteLine("----------------------------------------");

            foreach (var client in _clientService.GetAllClients())
            {
                Console.WriteLine(
                    $"{client.Id,-5} {client.FirstName} {client.LastName}");
            }

            Console.Write("\nChoose Client Id: ");
            int clientId = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("=== STAFF ===");



            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-20} {"Position",-15}");
            Console.WriteLine("--------------------------------------------------");
            foreach (var s in _staffService.GetAllStaff()) // not sure about this one
            {
                Console.WriteLine(
                $"{s.Id,-5} {s.Name,-20} {s.Position,-15}");
            }

            Console.Write("\nChoose Staff Id: ");
            int staffId = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("=== SERVICES ===");

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-20} {"Price",-10}");
            Console.WriteLine("--------------------------------------------------");

            foreach (var s in _serviceRepository.GetAll())
            {
                Console.WriteLine(
                $"{s.Id,-5} {s.Name,-20} {s.Price,-10}");
            }

            Console.Write("\nChoose Service Id: ");
            int serviceId = int.Parse(Console.ReadLine());

            Console.Write("Start (yyyy-MM-dd HH:mm): ");
            DateTime start = DateTime.Parse(Console.ReadLine());

            Console.Write("End (yyyy-MM-dd HH:mm): ");
            DateTime end = DateTime.Parse(Console.ReadLine());

            try
            {
                _appointmentService.CreateAppointment(
                    new Appointment
                    {
                        ClientId = clientId,
                        StaffId = staffId,
                        ServiceId = serviceId,
                        StartTime = start,
                        EndTime = end
                    });

                Console.WriteLine("Appointment created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        private void Edit()
        {
            Console.Clear();

            ListAllWithoutPause();

            Console.Write("\nAppointment Id: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("New Start: ");
            DateTime start = DateTime.Parse(Console.ReadLine());

            Console.Write("New End: ");
            DateTime end = DateTime.Parse(Console.ReadLine());

            var appointment = _appointmentService
                .GetAll()
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
            {
                Console.WriteLine("Appointment not found.");
                Console.ReadKey();
                return;
            }

            appointment.StartTime = start;
            appointment.EndTime = end;

            try
            {
                _appointmentService.UpdateAppointment(
                    appointment);

                Console.WriteLine("Appointment updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

        private void ListAll()
        {
            Console.Clear();

            ListAllWithoutPause();

            Console.ReadKey();
        }

        private void ListAllWithoutPause()
        {
            foreach (var a in _appointmentService.GetAll())
            {
                Console.WriteLine(
                    $"{a.Id} | Client:{a.ClientId} | Staff:{a.StaffId} | Service:{a.ServiceId}");

                Console.WriteLine(
                    $"{a.StartTime:g} -> {a.EndTime:g}");

                Console.WriteLine(
                    $"Status: {a.Status}");

                Console.WriteLine(
                    "----------------------------------");
            }
        }
    }
}