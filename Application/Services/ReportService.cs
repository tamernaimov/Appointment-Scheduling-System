using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Domain.Enums;
using Appointment_Scheduling_System.Infrastructure.Data;

namespace Appointment_Scheduling_System.Application.Services
{
    public class ReportService
    {
        private readonly JsonDataContext _context;

        public ReportService(JsonDataContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _context.Appointments
                .Where(a => a.StartTime.Date == date.Date)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByStatus(AppointmentStatus status)
        {
            return _context.Appointments
                .Where(a => a.Status == status)
                .ToList();
        }

        public int GetTotalAppointments()
        {
            return _context.Appointments.Count;
        }

        public int GetCompletedAppointments()
        {
            return _context.Appointments.Count(a => a.Status == AppointmentStatus.Completed);
        }
    }
}
