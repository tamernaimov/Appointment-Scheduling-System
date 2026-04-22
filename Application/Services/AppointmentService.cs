using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Domain.Enums;
using Appointment_Scheduling_System.Infrastructure.Data;

namespace Appointment_Scheduling_System.Application.Services
{
    public class AppointmentService
    {
        private readonly JsonDataContext _context;

        public AppointmentService(JsonDataContext context)
        {
            _context = context;
        }

        public void CreateAppointment(Appointment appointment)
        {
            // Проверка за конфликт (същия служител и overlapping време)
            bool hasConflict = _context.Appointments.Any(a =>
                a.StaffId == appointment.StaffId &&
                a.StartTime < appointment.EndTime &&
                appointment.StartTime < a.EndTime
            );

            if (hasConflict)
                throw new Exception("Time slot is already booked.");

            appointment.Id = _context.Appointments.Count + 1;
            appointment.Status = AppointmentStatus.Scheduled;

            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments;
        }

        public void CancelAppointment(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return;

            appointment.Status = AppointmentStatus.Cancelled;
            _context.SaveChanges();
        }

        public void CompleteAppointment(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return;

            appointment.Status = AppointmentStatus.Completed;
            _context.SaveChanges();
        }
    }
}
