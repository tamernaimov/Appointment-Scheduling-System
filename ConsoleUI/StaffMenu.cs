using Appointment_Scheduling_System.Application.Interfaces;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class StaffMenu
    {
        private readonly IStaffService _staffService;

        public StaffMenu(IStaffService staffService)
        {
            _staffService = staffService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Header("STAFF");

                Console.WriteLine("[1] Add Staff");
                Console.WriteLine("[2] List Staff");
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
            Header("ADD STAFF");

            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Position: ");
            var pos = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(pos))
            {
                Error("Invalid input");
                return;
            }

            _staffService.AddStaff(name, pos);
            Success("Staff added");
        }

        private void List()
        {
            Console.Clear();
            Header("STAFF LIST");

            foreach (var s in _staffService.GetAllStaff())
                Console.WriteLine($"{s.Id} | {s.Name} | {s.Position}");

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