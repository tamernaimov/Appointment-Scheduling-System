using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Enums;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ReportMenu : MenuBase
    {
        protected override ConsoleColor AccentColor => ConsoleColor.Cyan;

        private readonly IReportService _reportService;
        private readonly IStaffService _staffService;
        private readonly IServiceManagementService _serviceService;
        private readonly IClientService _clientService;

        public ReportMenu(
            IReportService reportService,
            IStaffService staffService,
            IServiceManagementService serviceService,
            IClientService clientService)
        {
            _reportService = reportService;
            _staffService = staffService;
            _serviceService = serviceService;
            _clientService = clientService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Header("REPORTS");

                Console.WriteLine("[1] Daily Schedule (by Staff)");
                Console.WriteLine("[2] Weekly Schedule (by Service)");
                Console.WriteLine("[3] Appointments By Status");
                Console.WriteLine("[4] Client History");
                Console.WriteLine("[5] Staff Workload");
                Console.WriteLine("[6] Most Booked Services");
                Console.WriteLine("[7] Revenue Report");
                Console.WriteLine("[8] Cancel / NoShow Stats");
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

        // ================= DAILY =================
        private void DailySchedule()
        {
            Console.Clear();
            Header("DAILY SCHEDULE");

            var staff = _staffService.GetAllStaff();
            PrintTable(
                new[] { "Id", "Name", "Position" },
                staff.Select(s => new[] { s.Id.ToString(), s.Name, s.Position }));

            int staffId = ReadInt("\nStaff Id");
            DateTime date = ReadDate("Date (yyyy-MM-dd)");

            var result = _reportService.GetDailySchedule(staffId, date);
            Print(result.Select(a => $"{a.StartTime:HH:mm} - {a.EndTime:HH:mm}"));

            Pause();
        }

        // ================= WEEKLY =================
        private void WeeklySchedule()
        {
            Console.Clear();
            Header("WEEKLY SCHEDULE");

            var services = _serviceService.GetAllServices();
            PrintTable(
                new[] { "Id", "Name" },
                services.Select(s => new[] { s.Id.ToString(), s.Name }));

            int serviceId = ReadInt("\nService Id");
            DateTime start = ReadDate("Week Start Date");

            var result = _reportService.GetWeeklySchedule(serviceId, start);
            Print(result.Select(a => $"{a.StartTime:dd/MM HH:mm} | Staff {a.StaffId}"));

            Pause();
        }

        // ================= STATUS =================
        private void ByStatus()
        {
            Console.Clear();
            Header("APPOINTMENTS BY STATUS");

            Console.WriteLine("Available statuses:");
            Console.WriteLine("Scheduled | Completed | Cancelled | NoShow");

            var status = ReadEnum<AppointmentStatus>("Status");

            var result = _reportService.GetAppointmentsByStatus(status);
            Print(result.Select(a => $"{a.Id} | {a.StartTime:dd/MM HH:mm} | {a.Status}"));

            Pause();
        }

        // ================= CLIENT HISTORY =================
        private void ClientHistory()
        {
            Console.Clear();
            Header("CLIENT HISTORY");

            var clients = _clientService.GetAllClients();
            PrintTable(
                new[] { "Id", "First Name", "Last Name" },
                clients.Select(c => new[] { c.Id.ToString(), c.FirstName, c.LastName }));

            int clientId = ReadInt("\nClient Id");

            var result = _reportService.GetClientHistory(clientId);
            Print(result.Select(a => $"{a.Id} | {a.StartTime:dd/MM HH:mm} | {a.Status}"));

            Pause();
        }

        // ================= STAFF WORKLOAD =================
        private void StaffWorkload()
        {
            Console.Clear();
            Header("STAFF WORKLOAD");

            var staff = _staffService.GetAllStaff();
            PrintTable(
                new[] { "Id", "Name" },
                staff.Select(s => new[] { s.Id.ToString(), s.Name }));

            int staffId = ReadInt("\nStaff Id");
            DateTime start = ReadDate("Start Date");
            DateTime end = ReadDate("End Date");

            int count = _reportService.GetStaffWorkload(staffId, start, end);
            Console.WriteLine($"\nTotal Appointments: {count}");

            Pause();
        }

        // ================= OTHERS =================
        private void MostBookedServices()
        {
            Console.Clear();
            Header("MOST BOOKED SERVICES");

            var result = _reportService.GetMostBookedServices();
            PrintTable(
                new[] { "Service Id", "Bookings" },
                result.Select(s => new[] { s.ServiceId.ToString(), s.Count.ToString() }));

            Pause();
        }

        private void Revenue()
        {
            Console.Clear();
            Header("REVENUE REPORT");

            DateTime start = ReadDate("Start Date");
            DateTime end = ReadDate("End Date");

            var revenue = _reportService.GetRevenue(start, end);
            Console.WriteLine($"\nTotal Revenue: {revenue}");

            Pause();
        }

        private void Stats()
        {
            Console.Clear();
            Header("CANCEL / NOSHOW STATS");

            var stats = _reportService.GetStats();

            Console.WriteLine($"Cancelled: {stats.Cancelled}");
            Console.WriteLine($"NoShow: {stats.NoShow}");

            Pause();
        }

        // ================= HELPER (специфичен само за Reports) =================
        private void Print(IEnumerable<string> lines)
        {
            Console.WriteLine();
            var list = lines.ToList();

            if (list.Count == 0)
            {
                Console.WriteLine("(няма резултати)");
                return;
            }

            foreach (var l in list)
                Console.WriteLine(l);

            Console.WriteLine($"\nВсичко: {list.Count}");
        }
    }
}