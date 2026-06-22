namespace Appointment_Scheduling_System.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; private set; }
        public int StaffId { get; private set; }
        public DayOfWeek DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        private Schedule() { } // EF

        public Schedule(int staffId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            if (staffId <= 0)
                throw new ArgumentException("Invalid staff id");

            StaffId = staffId;
            DayOfWeek = dayOfWeek;
            SetTimes(startTime, endTime);
        }

        public void SetTimes(TimeSpan startTime, TimeSpan endTime)
        {
            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time");

            StartTime = startTime;
            EndTime = endTime;
        }
    }
}