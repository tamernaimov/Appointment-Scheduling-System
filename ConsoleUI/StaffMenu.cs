using Appointment_Scheduling_System.Application.Interfaces;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class StaffMenu : MenuBase
    {
        protected override ConsoleColor AccentColor => ConsoleColor.Cyan;

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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pos))
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

            PrintTable(
                new[] { "Id", "Name", "Position" },
                _staffService.GetAllStaff().Select(s => new[] { s.Id.ToString(), s.Name, s.Position }));

            Pause();
        }
    }
}