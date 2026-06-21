using Appointment_Scheduling_System.ConsoleUI.Menus;

public class MainMenu : MenuBase
{
    protected override ConsoleColor AccentColor => ConsoleColor.White;

    private readonly ClientMenu _clients;
    private readonly StaffMenu _staff;
    private readonly ServiceMenu _services;
    private readonly AppointmentMenu _appointments;
    private readonly ReportMenu _reports;
    private readonly ScheduleMenu _schedule;

    public MainMenu(
        ClientMenu clients,
        StaffMenu staff,
        ServiceMenu services,
        AppointmentMenu appointments,
        ReportMenu reports,
        ScheduleMenu schedule)
    {
        _clients = clients;
        _staff = staff;
        _services = services;
        _appointments = appointments;
        _reports = reports;
        _schedule = schedule;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Header("APPOINTMENT SYSTEM");

            Console.WriteLine("[1] Clients");
            Console.WriteLine("[2] Services");
            Console.WriteLine("[3] Staff");
            Console.WriteLine("[4] Appointments");
            Console.WriteLine("[5] Reports");
            Console.WriteLine("[6] Schedule");
            Console.WriteLine("[0] Exit");

            Console.Write("\nSelect: ");

            switch (Console.ReadLine())
            {
                case "1": Safe(() => _clients.Show()); break;
                case "2": Safe(() => _services.Show()); break;
                case "3": Safe(() => _staff.Show()); break;
                case "4": Safe(() => _appointments.Show()); break;
                case "5": Safe(() => _reports.Show()); break;
                case "6": Safe(() => _schedule.Show()); break;
                case "0": return;
            }
        }
    }
}