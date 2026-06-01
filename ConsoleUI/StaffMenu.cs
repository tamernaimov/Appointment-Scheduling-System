using Appointment_Scheduling_System.Application.Services;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    public class StaffMenu
    {
        private readonly StaffService _staffService;

        public StaffMenu(StaffService staffService)
        {
            _staffService = staffService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Blue;

                Console.WriteLine("==================================================");
                Console.WriteLine("                     STAFF");
                Console.WriteLine("==================================================");

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine("[1] Add Staff");
                Console.WriteLine("[2] List Staff");

                Console.WriteLine();
                Console.WriteLine("[0] Back");

                Console.WriteLine();
                Console.Write("Choose option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStaff();
                        break;

                    case "2":
                        ListStaff();
                        break;

                    case "0":
                        return;
                }
            }
        }

        private void AddStaff()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Position: ");
            string position = Console.ReadLine();

            _staffService.AddStaff(name, position);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Staff Member added successfully.");
            Console.ResetColor();
            Console.ReadKey();
        }

        private void ListStaff()
        {
            var staffMembers = _staffService.GetAllStaff();

            foreach (var staff in staffMembers)
            {
                Console.WriteLine(
                    $"{staff.Id} | {staff.Name} | {staff.Position}");
            }

            Console.ReadKey();
        }
    }
}