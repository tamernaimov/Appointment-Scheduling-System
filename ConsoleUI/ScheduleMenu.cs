using Appointment_Scheduling_System.Application.Interfaces;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ScheduleMenu
    {
        private readonly IScheduleService _scheduleService;
        private readonly IStaffService _staffService;

        public ScheduleMenu(
            IScheduleService scheduleService,
            IStaffService staffService)
        {
            _scheduleService = scheduleService;
            _staffService = staffService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Header("SCHEDULE");

                Console.WriteLine("[1] Add Working Days");
                Console.WriteLine("[2] View Schedule");
                Console.WriteLine("[0] Back");

                Console.Write("\nSelect: ");

                switch (Console.ReadLine())
                {
                    case "1": Add(); break;
                    case "2": List(); break;
                    case "0": return;
                }
            }
        }

        private void Add()
        {
            Console.Clear();
            Header("ADD SCHEDULE");

            // ================= STAFF LIST =================
            Console.WriteLine("=== STAFF LIST ===");

            var staffList = _staffService.GetAllStaff();

            foreach (var s in staffList)
                Console.WriteLine($"{s.Id} | {s.Name} | {s.Position}");

            Console.Write("\nSelect Staff Id: ");

            if (!int.TryParse(Console.ReadLine(), out int staffId) ||
                !staffList.Any(x => x.Id == staffId))
            {
                Error("Invalid Staff Id");
                return;
            }

            // ================= DAYS =================
            Console.WriteLine("\nDays (1-7):");
            Console.WriteLine("1 Monday");
            Console.WriteLine("2 Tuesday");
            Console.WriteLine("3 Wednesday");
            Console.WriteLine("4 Thursday");
            Console.WriteLine("5 Friday");
            Console.WriteLine("6 Saturday");
            Console.WriteLine("7 Sunday");

            Console.Write("Start day: ");
            if (!int.TryParse(Console.ReadLine(), out int startDay) || startDay < 1 || startDay > 7)
            {
                Error("Invalid start day");
                return;
            }

            Console.Write("End day: ");
            if (!int.TryParse(Console.ReadLine(), out int endDay) || endDay < startDay || endDay > 7)
            {
                Error("Invalid end day");
                return;
            }

            // ================= TIME =================
            Console.Write("Start time (hh:mm): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out var startTime))
            {
                Error("Invalid start time");
                return;
            }

            Console.Write("End time (hh:mm): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out var endTime))
            {
                Error("Invalid end time");
                return;
            }

            if (startTime >= endTime)
            {
                Error("Start time must be before end time");
                return;
            }

            try
            {
                _scheduleService.AddScheduleRange(
                    staffId,
                    startDay,
                    endDay,
                    startTime,
                    endTime);

                Success("Schedule added successfully");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void List()
        {
            Console.Clear();
            Header("SCHEDULE LIST");

            foreach (var s in _scheduleService.GetAllSchedules())
                Console.WriteLine($"{s.Id} | Staff:{s.StaffId} | {s.DayOfWeek} | {s.StartTime}-{s.EndTime}");

            Pause();
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