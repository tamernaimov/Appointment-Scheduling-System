using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Domain.Enums;

namespace Appointment_Scheduling_System.Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public void CreateAppointment(Appointment appointment)
        {
            var existing = _repository.GetAll();

            bool hasConflict = existing.Any(a =>
                a.StaffId == appointment.StaffId &&
                a.StartTime < appointment.EndTime &&
                appointment.StartTime < a.EndTime
            );

            if (hasConflict)
                throw new Exception("Time slot is already booked.");

            appointment.Status = AppointmentStatus.Scheduled;

            _repository.Add(appointment);
        }

        public List<Appointment> GetAll()
        {
            return _repository.GetAll();
        }

        public void CancelAppointment(int id)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null) return;

            appointment.Status = AppointmentStatus.Cancelled;
            _repository.Update(appointment);
        }

        public void CompleteAppointment(int id)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null) return;

            appointment.Status = AppointmentStatus.Completed;
            _repository.Update(appointment);
        }
    }
}