using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

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

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                Console.WriteLine("==================================================");
                Console.WriteLine("                   SCHEDULE");
                Console.WriteLine("==================================================");

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine("[1] Add Working Days");
                Console.WriteLine("[2] View Schedule");

                Console.WriteLine();
                Console.WriteLine("[0] Back");

                Console.WriteLine();
                Console.Write("Choose option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Add();
                        break;

                    case "2":
                        List();
                        break;

                    case "0":
                        return;
                }
            }
        }

        private void Add()
        {
            Console.Write("Staff Id: ");
            if (!int.TryParse(Console.ReadLine(), out int staffId))
            {
                Console.WriteLine("Invalid Id.");
                Console.ReadKey();
                return;
            }

            Console.Write("Start day (1-7): ");
            int startDay = int.Parse(Console.ReadLine());

            Console.Write("End day (1-7): ");
            int endDay = int.Parse(Console.ReadLine());

            Console.Write("Start time (hh:mm): ");
            TimeSpan startTime = TimeSpan.Parse(Console.ReadLine());

            Console.Write("End time (hh:mm): ");
            TimeSpan endTime = TimeSpan.Parse(Console.ReadLine());

            try
            {
                _scheduleService.AddScheduleRange(
                    staffId,
                    startDay,
                    endDay,
                    startTime,
                    endTime);

                Console.WriteLine("Schedule added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        private void List()
        {
            var schedules = _scheduleService.GetAllSchedules();

            foreach (var s in schedules)
            {
                Console.WriteLine($"{s.Id} | Staff:{s.StaffId} | {s.DayOfWeek} | {s.StartTime}-{s.EndTime}");
            }

            Console.ReadKey();
        }
    }
}