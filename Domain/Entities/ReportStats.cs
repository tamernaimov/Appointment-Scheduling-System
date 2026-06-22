namespace Appointment_Scheduling_System.Domain.Entities
{
    public class ReportStats
    {
        public int Cancelled { get; }
        public int NoShow { get; }

        public ReportStats(int cancelled, int noShow)
        {
            Cancelled = cancelled;
            NoShow = noShow;
        }
    }
}