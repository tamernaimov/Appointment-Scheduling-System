using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Application.Services;

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

                Console.ForegroundColor = ConsoleColor.DarkCyan;

                Console.WriteLine("==================================================");
                Console.WriteLine("                    REPORTS");
                Console.WriteLine("==================================================");

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine("[1] Daily Schedule");
                Console.WriteLine("[2] Weekly Schedule");
                Console.WriteLine("[3] Appointments By Status");
                Console.WriteLine("[4] Client History");
                Console.WriteLine("[5] Staff Workload");
                Console.WriteLine("[6] Most Booked Services");
                Console.WriteLine("[7] Revenue Report");
                Console.WriteLine("[8] Cancelled / NoShow Stats");

                Console.WriteLine();
                Console.WriteLine("[0] Back");

                Console.WriteLine();
                Console.Write("Choose option: ");
                var choice = Console.ReadLine();
                switch (choice) { 
                case "1":
                    DailySchedule();
                    break;

                case "2":
                    WeeklySchedule();
                    break;

                case "3":
                    ByStatus();
                    break;

                case "4":
                    ClientHistory();
                    break;

                case "5":
                    StaffWorkload();
                    break;

                case "6":
                    MostBookedServices();
                    break;

                case "7":
                    Revenue();
                    break;

                case "8":
                    Stats();
                    break;
                }
            }
        }
        void ByDate()
        {
            Console.Write("Date (yyyy-mm-dd): ");
            var date = DateTime.Parse(Console.ReadLine());

            var result = _reportService.GetAppointmentsByDate(date);

            foreach (var a in result)
                Console.WriteLine($"{a.Id} | {a.StartTime} | {a.Status}");

            Console.ReadKey();
        }

        void ByStatus()
        {
            Console.Write("Status (Scheduled/Completed/Cancelled/NoShow): ");
            var status = Enum.Parse<Domain.Enums.AppointmentStatus>(Console.ReadLine());

            var result = _reportService.GetAppointmentsByStatus(status);

            foreach (var a in result)
                Console.WriteLine($"{a.Id} | {a.Status}");

            Console.ReadKey();
        }

        void ClientHistory()
        {
            Console.Write("Client ID: ");
            int id = int.Parse(Console.ReadLine());

            var result = _reportService.GetClientHistory(id);

            foreach (var a in result)
                Console.WriteLine($"{a.Id} | {a.StartTime} | {a.Status}");

            Console.ReadKey();
        }
        void Revenue()
        {
            Console.Write("Start date: ");
            var start = DateTime.Parse(Console.ReadLine());

            Console.Write("End date: ");
            var end = DateTime.Parse(Console.ReadLine());

            var revenue = _reportService.GetRevenue(start, end);

            Console.WriteLine($"Revenue: {revenue}");

            Console.ReadKey();
        }
        private void DailySchedule()
        {
            Console.Write("Staff Id: ");
            int staffId = int.Parse(Console.ReadLine());

            Console.Write("Date: ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            var appointments =
                _reportService.GetDailySchedule(staffId, date);

            foreach (var a in appointments)
            {
                Console.WriteLine(
                    $"{a.StartTime} - {a.EndTime}");
            }

            Console.ReadKey();
        }
        private void WeeklySchedule()
        {
            Console.Write("Service Id: ");
            int serviceId = int.Parse(Console.ReadLine());

            Console.Write("Week Start Date: ");
            DateTime start = DateTime.Parse(Console.ReadLine());

            var appointments =
                _reportService.GetWeeklySchedule(serviceId, start);

            foreach (var a in appointments)
            {
                Console.WriteLine(
                    $"{a.StartTime} | Staff {a.StaffId}");
            }

            Console.ReadKey();
        }
        private void StaffWorkload()
        {
            Console.Write("Staff Id: ");
            int staffId = int.Parse(Console.ReadLine());

            Console.Write("Start Date: ");
            DateTime start = DateTime.Parse(Console.ReadLine());

            Console.Write("End Date: ");
            DateTime end = DateTime.Parse(Console.ReadLine());

            int count =
                _reportService.GetStaffWorkload(
                    staffId,
                    start,
                    end);

            Console.WriteLine(
                $"Appointments: {count}");

            Console.ReadKey();
        }
        private void MostBookedServices()
        {
            var services =
                _reportService.GetMostBookedServices();

            foreach (var service in services)
            {
                Console.WriteLine(
                    $"Service {service.ServiceId} -> {service.Count}");
            }

            Console.ReadKey();
        }

        void Stats()
        {
            var stats = _reportService.GetStats();

            Console.WriteLine($"Cancelled: {stats.Cancelled}");
            Console.WriteLine($"NoShow: {stats.NoShow}");

            Console.ReadKey();
        }
    }
}