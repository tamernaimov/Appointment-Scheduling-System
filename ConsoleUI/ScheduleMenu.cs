using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class ScheduleMenu
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleMenu(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
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
            int staffId = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("Days:");
            Console.WriteLine("1 = Monday");
            Console.WriteLine("2 = Tuesday");
            Console.WriteLine("3 = Wednesday");
            Console.WriteLine("4 = Thursday");
            Console.WriteLine("5 = Friday");
            Console.WriteLine("6 = Saturday");
            Console.WriteLine("7 = Sunday");

            Console.Write("Start day: ");
            int startDay = int.Parse(Console.ReadLine());

            Console.Write("End day: ");
            int endDay = int.Parse(Console.ReadLine());

            Console.Write("Start time (hh:mm): ");
            TimeSpan startTime = TimeSpan.Parse(Console.ReadLine());

            Console.Write("End time (hh:mm): ");
            TimeSpan endTime = TimeSpan.Parse(Console.ReadLine());

            for (int day = startDay; day <= endDay; day++)
            {
                DayOfWeek dayOfWeek =
                    day == 7
                        ? DayOfWeek.Sunday
                        : (DayOfWeek)day;

                _scheduleRepository.Add(new Schedule
                {
                    StaffId = staffId,
                    DayOfWeek = dayOfWeek,
                    StartTime = startTime,
                    EndTime = endTime
                });
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Schedule added successfully.");
            Console.ResetColor();
            Console.ReadKey();
        }

        void List()
        {
            foreach (var s in _scheduleRepository.GetAll())
            {
                Console.WriteLine($"{s.Id} | Staff:{s.StaffId} | {s.DayOfWeek} | {s.StartTime}-{s.EndTime}");
            }

            Console.ReadKey();
        }
    }
}