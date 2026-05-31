namespace Appointment_Scheduling_System.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }

        public int StaffId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}