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
                Console.WriteLine("[1] Add Staff");
                Console.WriteLine("[2] List Staff");
                Console.WriteLine("[0] Back");

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

        void Add()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Position: ");
            var pos = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(pos))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            _staffService.AddStaff(name, pos);
        }

        void List()
        {
            foreach (var s in _staffService.GetAllStaff())
                Console.WriteLine($"{s.Id} {s.Name} {s.Position}");

            Console.ReadKey();
        }
    }
}