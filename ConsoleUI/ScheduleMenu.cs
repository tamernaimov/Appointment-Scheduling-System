using Appointment_Scheduling_System.Application.Interfaces;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ScheduleMenu : MenuBase
    {
        protected override ConsoleColor AccentColor => ConsoleColor.Cyan;

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

            var staffList = _staffService.GetAllStaff();
            PrintTable(
                new[] { "Id", "Name", "Position" },
                staffList.Select(s => new[] { s.Id.ToString(), s.Name, s.Position }));

            Console.Write("\n");
            int staffId = ReadInt("Select Staff Id",
                id => staffList.Any(x => x.Id == id),
                "Няма служител с такъв Id.");

            Console.WriteLine("\nDays (1-7):");
            Console.WriteLine("1 Monday");
            Console.WriteLine("2 Tuesday");
            Console.WriteLine("3 Wednesday");
            Console.WriteLine("4 Thursday");
            Console.WriteLine("5 Friday");
            Console.WriteLine("6 Saturday");
            Console.WriteLine("7 Sunday");

            int startDay = ReadInt("Start day", d => d >= 1 && d <= 7, "Денят трябва да е между 1 и 7.");
            int endDay = ReadInt("End day",
                d => d >= startDay && d <= 7,
                $"Крайният ден трябва да е между {startDay} и 7.");

            TimeSpan startTime = ReadTimeSpan("Start time (hh:mm)");
            TimeSpan endTime = ReadTimeSpan("End time (hh:mm)",
                t => t > startTime,
                "Часът на край трябва да е след началото.");

            TryRun(() => _scheduleService.AddScheduleRange(
                staffId,
                startDay,
                endDay,
                startTime,
                endTime), "Schedule added successfully");
        }

        private void List()
        {
            Console.Clear();
            Header("SCHEDULE LIST");

            PrintTable(
                new[] { "Id", "Staff", "Day", "Start", "End" },
                _scheduleService.GetAllSchedules().Select(s =>
                    new[] { s.Id.ToString(), s.StaffId.ToString(), s.DayOfWeek.ToString(), s.StartTime.ToString(), s.EndTime.ToString() }));

            Pause();
        }
    }
}