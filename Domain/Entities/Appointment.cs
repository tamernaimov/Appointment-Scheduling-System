using Appointment_Scheduling_System.Domain.Enums;

namespace Appointment_Scheduling_System.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; private set; }

        public int ClientId { get; private set; }
        public int StaffId { get; private set; }
        public int ServiceId { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public AppointmentStatus Status { get; private set; }

        private Appointment() { } // EF Core

        public Appointment(int clientId, int staffId, int serviceId, DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time.");

            if (startTime < DateTime.Now)
                throw new ArgumentException("Cannot create appointment in the past.");

            ClientId = clientId;
            StaffId = staffId;
            ServiceId = serviceId;
            StartTime = startTime;
            EndTime = endTime;

            Status = AppointmentStatus.Scheduled;
        }

        public void Cancel()
        {
            if ((StartTime - DateTime.Now).TotalHours < 24)
                throw new InvalidOperationException("Cannot cancel within 24h.");

            Status = AppointmentStatus.Cancelled;
        }

        public void Complete()
        {
            if (Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException("Cancelled cannot be completed.");

            Status = AppointmentStatus.Completed;
        }

        public void MarkNoShow()
        {
            if (Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException("Cancelled cannot be NoShow.");

            Status = AppointmentStatus.NoShow;
        }

        public void Reschedule(DateTime newStart, DateTime newEnd)
        {
            if (newStart >= newEnd)
                throw new ArgumentException("Invalid time range.");

            StartTime = newStart;
            EndTime = newEnd;
        }
    }
}