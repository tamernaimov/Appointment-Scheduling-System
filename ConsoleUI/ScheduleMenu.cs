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

            Console.Write("\nSelect Staff Id: ");
            if (!int.TryParse(Console.ReadLine(), out int staffId) ||
                !staffList.Any(x => x.Id == staffId))
            {
                Error("Invalid Staff Id");
                return;
            }

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

            PrintTable(
                new[] { "Id", "Staff", "Day", "Start", "End" },
                _scheduleService.GetAllSchedules().Select(s =>
                    new[] { s.Id.ToString(), s.StaffId.ToString(), s.DayOfWeek.ToString(), s.StartTime.ToString(), s.EndTime.ToString() }));

            Pause();
        }
    }
}