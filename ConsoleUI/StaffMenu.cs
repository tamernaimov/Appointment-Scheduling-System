
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
                Console.WriteLine("[3] Delete Staff");
                Console.WriteLine("[0] Back");
                Console.Write("\nSelect: ");
                switch (Console.ReadLine())
                {
                    case "1": Add(); break;
                    case "2": List(); break;
                    case "3": Delete(); break;
                    case "0": return;
                }
            }
        }

        private void Add()
        {
            Console.Clear();
            Header("ADD STAFF");

            var name = ReadRequired("Name");
            var pos = ReadRequired("Position");

            TryRun(() => _staffService.AddStaff(name, pos), "Staff added");
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

        private void Delete()
        {
            Console.Clear();
            Header("DELETE STAFF");

            var staff = _staffService.GetAllStaff();
            PrintTable(
                new[] { "Id", "Name", "Position" },
                staff.Select(s => new[] { s.Id.ToString(), s.Name, s.Position }));

            if (staff.Count == 0)
            {
                Pause();
                return;
            }

            int id = ReadInt("\nStaff Id", sid => staff.Any(x => x.Id == sid), "Няма служител с такъв Id.");
            var s = staff.First(x => x.Id == id);

            if (!Confirm($"\nИзтрий {s.Name}?"))
                return;

            TryRun(() => _staffService.DeleteStaff(id), "Staff deleted");
        }
    }
}