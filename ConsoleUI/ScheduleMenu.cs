using Appointment_Scheduling_System.Application.Interfaces;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ScheduleMenu
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleMenu(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
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

            Console.Write("Staff Id: ");
            if (!int.TryParse(Console.ReadLine(), out int staffId))
            {
                Error("Invalid Id");
                return;
            }

            Console.Write("Start day (1-7): ");
            int startDay = int.Parse(Console.ReadLine());

            Console.Write("End day (1-7): ");
            int endDay = int.Parse(Console.ReadLine());

            Console.Write("Start time: ");
            var start = TimeSpan.Parse(Console.ReadLine());

            Console.Write("End time: ");
            var end = TimeSpan.Parse(Console.ReadLine());

            try
            {
                _scheduleService.AddScheduleRange(staffId, startDay, endDay, start, end);
                Success("Schedule added");
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