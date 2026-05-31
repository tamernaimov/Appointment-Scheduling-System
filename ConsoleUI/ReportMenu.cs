using Appointment_Scheduling_System.Application.Services;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ReportMenu
    {
        private readonly ReportService _reportService;

        public ReportMenu(ReportService reportService)
        {
            _reportService = reportService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== REPORT MENU ===");
                Console.WriteLine("1. Appointments by date");
                Console.WriteLine("2. Appointments by status");
                Console.WriteLine("3. Client history");
                Console.WriteLine("4. Revenue report");
                Console.WriteLine("5. Stats (Cancelled / NoShow)");
                Console.WriteLine("0. Back");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ByDate();
                        break;

                    case "2":
                        ByStatus();
                        break;

                    case "3":
                        ClientHistory();
                        break;

                    case "4":
                        Revenue();
                        break;

                    case "5":
                        Stats();
                        break;

                    case "0":
                        return;
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

        void Stats()
        {
            var stats = _reportService.GetStats();

            Console.WriteLine($"Cancelled: {stats.Cancelled}");
            Console.WriteLine($"NoShow: {stats.NoShow}");

            Console.ReadKey();
        }
    }
}