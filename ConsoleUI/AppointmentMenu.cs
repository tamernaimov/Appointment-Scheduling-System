using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.ConsoleUI.Helpers;
using System;
using System.Linq;
using System.Text;

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
                Header("APPOINTMENTS");

                Console.WriteLine("[1] Create Appointment");
                Console.WriteLine("[2] Edit Appointment");
                Console.WriteLine("[3] Cancel Appointment");
                Console.WriteLine("[4] Complete Appointment");
                Console.WriteLine("[5] Mark NoShow");
                Console.WriteLine("[6] List All");
                Console.WriteLine("[0] Back");

                Console.Write("\nSelect: ");

                switch (Console.ReadLine())
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
            Header("CREATE APPOINTMENT");

            Console.WriteLine("Clients:");
            foreach (var c in _clientService.GetAllClients())
                Console.WriteLine($"{c.Id} | {c.FirstName} {c.LastName}");

            Console.Write("\nClient Id: ");
            if (!int.TryParse(Console.ReadLine(), out int clientId))
                return;

            Console.WriteLine("\nStaff:");
            foreach (var s in _staffService.GetAllStaff())
                Console.WriteLine($"{s.Id} | {s.Name}");

            Console.Write("\nStaff Id: ");
            if (!int.TryParse(Console.ReadLine(), out int staffId))
                return;

            Console.WriteLine("\nServices:");
            foreach (var s in _serviceService.GetAllServices())
                Console.WriteLine($"{s.Id} | {s.Name} | {s.Price}");

            Console.Write("\nService Id: ");
            if (!int.TryParse(Console.ReadLine(), out int serviceId))
                return;

            var startPick = ConsoleDateTimePicker.Pick("ИЗБОР НА НАЧАЛО");
            if (startPick == null) return;
            DateTime start = startPick.Value;

            var endPick = ConsoleDateTimePicker.Pick("ИЗБОР НА КРАЙ", start); // тръгва от началната дата
            if (endPick == null) return;
            DateTime end = endPick.Value;
            try
            {
                _appointmentService.CreateAppointment(clientId, staffId, serviceId, start, end);
                Success("Appointment created");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

            
        

        private void Edit()
        {
            Console.Clear();
            Header("EDIT APPOINTMENT");

            ListAllInternal();

            Console.Write("\nAppointment Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            Console.Write("New Start: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var start)) return;

            Console.Write("New End: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var end)) return;

            var appointment = _appointmentService.GetAll()
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
            {
                Error("Not found");
                return;
            }

            try
            {
                appointment.Reschedule(start, end);
                _appointmentService.UpdateAppointment(appointment);
                Success("Updated");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void Cancel()
        {
            Console.Clear();
            ListAllInternal();
            Header("CANCEL APPOINTMENT");

            Console.Write("Appointment Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            try
            {
                _appointmentService.CancelAppointment(id);
                Success("Cancelled");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void Complete()
        {
            Console.Clear();

            ListAllInternal();
            Header("COMPLETE APPOINTMENT");

            Console.Write("Appointment Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            try
            {
                _appointmentService.CompleteAppointment(id);
                Success("Completed");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void NoShow()
        {
            Console.Clear();
            ListAllInternal();
            Header("MARK NOSHOW");

            Console.Write("Appointment Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            try
            {
                _appointmentService.MarkAsNoShow(id);
                Success("Marked as NoShow");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void ListAll()
        {
            Console.Clear();
            Header("ALL APPOINTMENTS");

            ListAllInternal();
            Pause();
        }

        private void ListAllInternal()
        {
            foreach (var a in _appointmentService.GetAll())
            {
                Console.WriteLine($"[{a.Id}] C:{a.ClientId} S:{a.StaffId} Ser:{a.ServiceId}");
                Console.WriteLine($"{a.StartTime:dd/MM HH:mm} - {a.EndTime:HH:mm}");
                Console.WriteLine($"Status: {a.Status}");
                Console.WriteLine("----------------------------------");
            }
        }

        private void Header(string t)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine($"          {t}");
            Console.WriteLine("==================================");
            Console.ResetColor();
        }

        private void Success(string m)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {m}");
            Console.ResetColor();
            Pause();
        }

        private void Error(string m)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {m}");
            Console.ResetColor();
            Pause();
        }

        private void Pause() => Console.ReadKey();
    }
}