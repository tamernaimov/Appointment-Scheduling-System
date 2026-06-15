using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Enums;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ReportMenu
    {
        private readonly IReportService _reportService;

        public ReportMenu(IReportService reportService)
        {
            _reportService = reportService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Header("REPORTS");

                Console.WriteLine("[1] Daily Schedule");
                Console.WriteLine("[2] Weekly Schedule");
                Console.WriteLine("[3] Appointments By Status");
                Console.WriteLine("[4] Client History");
                Console.WriteLine("[5] Staff Workload");
                Console.WriteLine("[6] Most Booked Services");
                Console.WriteLine("[7] Revenue Report");
                Console.WriteLine("[8] Cancelled / NoShow Stats");
                Console.WriteLine("[0] Back");

                Console.Write("\nSelect: ");

                switch (Console.ReadLine())
                {
                    case "1": DailySchedule(); break;
                    case "2": WeeklySchedule(); break;
                    case "3": ByStatus(); break;
                    case "4": ClientHistory(); break;
                    case "5": StaffWorkload(); break;
                    case "6": MostBookedServices(); break;
                    case "7": Revenue(); break;
                    case "8": Stats(); break;
                    case "0": return;
                }
            }
        }

        private void DailySchedule()
        {
            Console.Clear();
            Header("DAILY SCHEDULE");

            Console.Write("Staff Id: ");
            if (!int.TryParse(Console.ReadLine(), out int staffId))
            {
                Error("Invalid Staff Id");
                return;
            }

            Console.Write("Date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var date))
            {
                Error("Invalid Date");
                return;
            }

            var appointments = _reportService.GetDailySchedule(staffId, date);

            Console.WriteLine("\nAppointments:");
            foreach (var a in appointments)
                Console.WriteLine($"{a.StartTime:HH:mm} - {a.EndTime:HH:mm}");

            Pause();
        }

        private void WeeklySchedule()
        {
            Console.Clear();
            Header("WEEKLY SCHEDULE");

            Console.Write("Service Id: ");
            if (!int.TryParse(Console.ReadLine(), out int serviceId))
            {
                Error("Invalid Service Id");
                return;
            }

            Console.Write("Week Start Date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var start))
            {
                Error("Invalid Date");
                return;
            }

            var appointments = _reportService.GetWeeklySchedule(serviceId, start);

            Console.WriteLine("\nAppointments:");
            foreach (var a in appointments)
                Console.WriteLine($"{a.StartTime:dd/MM HH:mm} | Staff {a.StaffId}");

            Pause();
        }

        private void ByStatus()
        {
            Console.Clear();
            Header("APPOINTMENTS BY STATUS");

            Console.Write("Status (Scheduled/Completed/Cancelled/NoShow): ");
            var input = Console.ReadLine();

            if (!Enum.TryParse<AppointmentStatus>(input, out var status))
            {
                Error("Invalid Status");
                return;
            }

            var result = _reportService.GetAppointmentsByStatus(status);

            Console.WriteLine("\nResults:");
            foreach (var a in result)
                Console.WriteLine($"{a.Id} | {a.Status}");

            Pause();
        }

        private void ClientHistory()
        {
            Console.Clear();
            Header("CLIENT HISTORY");

            Console.Write("Client Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Error("Invalid Client Id");
                return;
            }

            var result = _reportService.GetClientHistory(id);

            Console.WriteLine("\nHistory:");
            foreach (var a in result)
                Console.WriteLine($"{a.Id} | {a.StartTime:dd/MM HH:mm} | {a.Status}");

            Pause();
        }

        private void StaffWorkload()
        {
            Console.Clear();
            Header("STAFF WORKLOAD");

            Console.Write("Staff Id: ");
            if (!int.TryParse(Console.ReadLine(), out int staffId))
            {
                Error("Invalid Staff Id");
                return;
            }

            Console.Write("Start Date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var start))
            {
                Error("Invalid Date");
                return;
            }

            Console.Write("End Date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var end))
            {
                Error("Invalid Date");
                return;
            }

            var count = _reportService.GetStaffWorkload(staffId, start, end);

            Console.WriteLine($"\nTotal Appointments: {count}");

            Pause();
        }

        private void MostBookedServices()
        {
            Console.Clear();
            Header("MOST BOOKED SERVICES");

            var services = _reportService.GetMostBookedServices();

            Console.WriteLine("\nServices:");
            foreach (var s in services)
                Console.WriteLine($"Service {s.ServiceId} -> {s.Count}");

            Pause();
        }

        private void Revenue()
        {
            Console.Clear();
            Header("REVENUE REPORT");

            Console.Write("Start Date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var start))
            {
                Error("Invalid Date");
                return;
            }

            Console.Write("End Date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var end))
            {
                Error("Invalid Date");
                return;
            }

            var revenue = _reportService.GetRevenue(start, end);

            Console.WriteLine($"\nTotal Revenue: {revenue}");

            Pause();
        }

        private void Stats()
        {
            Console.Clear();
            Header("CANCEL / NOSHOW STATS");

            var stats = _reportService.GetStats();

            Console.WriteLine($"\nCancelled: {stats.Cancelled}");
            Console.WriteLine($"NoShow: {stats.NoShow}");

            Pause();
        }

        // ================= UI HELPERS =================

        private void Header(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine($"          {title}");
            Console.WriteLine("==================================");
            Console.ResetColor();
        }

        private void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {message}");
            Console.ResetColor();
            Pause();
        }

        private void Pause() => Console.ReadKey();
    }
}