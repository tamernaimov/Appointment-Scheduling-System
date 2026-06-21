using Appointment_Scheduling_System.Application.Interfaces;
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

            var startPick = Pick("ИЗБОР НА НАЧАЛО");
            if (startPick == null) return;
            DateTime start = startPick.Value;

            var endPick = Pick("ИЗБОР НА КРАЙ", start); // тръгва от началната дата
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

            private enum Step { Date, Time }

            public static DateTime? Pick(string title, DateTime? initial = null)
            {
                DateTime date = (initial ?? DateTime.Now).Date;
                int hour = (initial ?? DateTime.Now).Hour;
                int minute = (initial ?? DateTime.Now).Minute;

                var step = Step.Date;
                bool editingHour = true;

                while (true)
                {
                    Draw(title, date, hour, minute, step, editingHour);
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                        return null;

                    if (step == Step.Date)
                    {
                        switch (key)
                        {
                            case ConsoleKey.LeftArrow: date = date.AddDays(-1); break;
                            case ConsoleKey.RightArrow: date = date.AddDays(1); break;
                            case ConsoleKey.UpArrow: date = date.AddDays(-7); break;
                            case ConsoleKey.DownArrow: date = date.AddDays(7); break;
                            case ConsoleKey.PageUp: date = date.AddMonths(-1); break;
                            case ConsoleKey.PageDown: date = date.AddMonths(1); break;
                            case ConsoleKey.Tab:
                            case ConsoleKey.Enter:
                                step = Step.Time;
                                break;
                        }
                    }
                    else // Step.Time
                    {
                        switch (key)
                        {
                            case ConsoleKey.LeftArrow:
                            case ConsoleKey.RightArrow:
                                editingHour = !editingHour;
                                break;
                            case ConsoleKey.UpArrow:
                                if (editingHour) hour = (hour + 1) % 24;
                                else minute = (minute + 5) % 60;
                                break;
                            case ConsoleKey.DownArrow:
                                if (editingHour) hour = (hour + 23) % 24;
                                else minute = (minute + 55) % 60;
                                break;
                            case ConsoleKey.Tab:
                                step = Step.Date;
                                break;
                            case ConsoleKey.Enter:
                                return date.AddHours(hour).AddMinutes(minute);
                        }
                    }
                }
            }
        

            private static void Draw(string title, DateTime cursor, int hour, int minute, Step step, bool editingHour)
            {
                Console.Clear();
                Console.WriteLine(title);
                Console.WriteLine(new string('=', title.Length));
                Console.WriteLine();

                var firstOfMonth = new DateTime(cursor.Year, cursor.Month, 1);
                Console.WriteLine($"   {firstOfMonth:MMMM yyyy}".ToUpper());
                Console.WriteLine(" Mo Tu We Th Fr Sa Su");

                int daysInMonth = DateTime.DaysInMonth(cursor.Year, cursor.Month);
                int startOffset = ((int)firstOfMonth.DayOfWeek + 6) % 7; // Monday = 0

                var sb = new StringBuilder();
                for (int i = 0; i < startOffset; i++)
                    sb.Append("   ");

                for (int day = 1; day <= daysInMonth; day++)
                {
                    string dayStr = day.ToString().PadLeft(2);
                    bool isSelected = step == Step.Date && day == cursor.Day;
                    sb.Append(isSelected ? $"[{dayStr}]" : $" {dayStr} ");

                    if ((startOffset + day) % 7 == 0)
                    {
                        Console.WriteLine(sb.ToString());
                        sb.Clear();
                    }
                }
                if (sb.Length > 0)
                    Console.WriteLine(sb.ToString());

                Console.WriteLine();
                string hourStr = step == Step.Time && editingHour ? $"[{hour:00}]" : $" {hour:00} ";
                string minStr = step == Step.Time && !editingHour ? $"[{minute:00}]" : $" {minute:00} ";
                Console.WriteLine($"Час:   {hourStr}:{minStr}");
                Console.WriteLine();

                Console.WriteLine(step == Step.Date
                    ? "Стрелки = ден | PgUp/PgDn = месец | Enter/Tab = към часа | Esc = отказ"
                    : "←/→ = час/минута | ↑/↓ = промяна | Tab = назад към дата | Enter = потвърди | Esc = отказ");
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