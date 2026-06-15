using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using System;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class AppointmentMenu
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IClientService _clientService;
        private readonly IStaffService _staffService;
        private readonly IServiceManagementService _serviceService;

        public AppointmentMenu(
            IAppointmentService appointmentService,
            IClientService clientService,
            IStaffService staffService,
            IServiceManagementService serviceService)
        {
            _appointmentService = appointmentService;
            _clientService = clientService;
            _staffService = staffService;
            _serviceService = serviceService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("==================================================");
                Console.WriteLine("                 APPOINTMENTS");
                Console.WriteLine("==================================================");

                Console.WriteLine("[1] Create Appointment");
                Console.WriteLine("[2] Edit Appointment");
                Console.WriteLine("[3] Cancel Appointment");
                Console.WriteLine("[4] Complete Appointment");
                Console.WriteLine("[5] Mark NoShow");
                Console.WriteLine("[6] List All");
                Console.WriteLine("[0] Back");

                Console.Write("\nChoose option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Create(); break;
                    case "2": Edit(); break;
                    case "3": Cancel(); break;
                    case "4": Complete(); break;
                    case "5": NoShow(); break;
                    case "6": ListAll(); break;
                    case "0": return;
                }
            }
        }

        private void Create()
        {
            Console.Clear();

            Console.WriteLine("=== CLIENTS ===");
            foreach (var c in _clientService.GetAllClients())
                Console.WriteLine($"{c.Id} {c.FirstName} {c.LastName}");

            Console.Write("\nClient Id: ");
            if (!int.TryParse(Console.ReadLine(), out int clientId))
                return;

            Console.WriteLine("\n=== STAFF ===");
            foreach (var s in _staffService.GetAllStaff())
                Console.WriteLine($"{s.Id} {s.Name}");

            Console.Write("\nStaff Id: ");
            if (!int.TryParse(Console.ReadLine(), out int staffId))
                return;

            Console.WriteLine("\n=== SERVICES ===");
            foreach (var s in _serviceService.GetAllServices())
                Console.WriteLine($"{s.Id} {s.Name} {s.Price}");

            Console.Write("\nService Id: ");
            if (!int.TryParse(Console.ReadLine(), out int serviceId))
                return;

            Console.Write("Start (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime start))
                return;

            Console.Write("End (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime end))
                return;

            try
            {
                _appointmentService.CreateAppointment(
                    clientId,
                    staffId,
                    serviceId,
                    start,
                    end
                );

                Console.WriteLine("Appointment created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        private void Edit()
        {
            ListAllWithoutPause();

            Console.Write("\nAppointment Id: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("New Start: ");
            DateTime start = DateTime.Parse(Console.ReadLine());

            Console.Write("New End: ");
            DateTime end = DateTime.Parse(Console.ReadLine());

            var appointment = _appointmentService.GetAll()
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
            {
                Console.WriteLine("Not found");
                return;
            }

            try
            {
                appointment.Reschedule(start, end);

                _appointmentService.UpdateAppointment(appointment);

                Console.WriteLine("Updated");
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
            if (!int.TryParse(Console.ReadLine(), out int id))
                return;

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
            if (!int.TryParse(Console.ReadLine(), out int id))
                return;

            try
            {
                _appointmentService.CompleteAppointment(id);
                Console.WriteLine("Completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        private void NoShow()
        {
            Console.Write("Appointment Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                return;

            try
            {
                _appointmentService.MarkAsNoShow(id);
                Console.WriteLine("Marked as NoShow.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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
                Console.WriteLine($"[{a.Id}] C:{a.ClientId} S:{a.StaffId} Ser:{a.ServiceId}");
                Console.WriteLine($"{a.StartTime:dd/MM HH:mm} - {a.EndTime:HH:mm}");
                Console.WriteLine($"Status: {a.Status}");
                Console.WriteLine("----------------------------------------");
            }
        }
    }
}