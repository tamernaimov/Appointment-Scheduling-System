using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Domain.Enums;

namespace Appointment_Scheduling_System.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IScheduleRepository scheduleRepository)
        {
            _appointmentRepository = appointmentRepository;
            _scheduleRepository = scheduleRepository;
        }

        public void CreateAppointment(Appointment appointment)
        {
            ValidateAppointmentTimes(appointment);
            ValidateNotInPast(appointment);
            ValidateWorkingHours(appointment);
            ValidateNoConflict(appointment);

            appointment.Status = AppointmentStatus.Scheduled;

            _appointmentRepository.Add(appointment);
        }

        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
        }

        public void CancelAppointment(int id)
        {
            var appointment = GetOrThrow(id);

            if (appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Completed appointment cannot be cancelled.");

            if (appointment.StartTime <= DateTime.Now)
                throw new InvalidOperationException("Past appointment cannot be cancelled.");

            if ((appointment.StartTime - DateTime.Now).TotalHours < 24)
                throw new InvalidOperationException("Must cancel at least 24 hours before.");

            appointment.Status = AppointmentStatus.Cancelled;

            _appointmentRepository.Update(appointment);
        }

        public void CompleteAppointment(int id)
        {
            var appointment = GetOrThrow(id);

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException("Cancelled appointment cannot be completed.");

            appointment.Status = AppointmentStatus.Completed;

            _appointmentRepository.Update(appointment);
        }

        public void UpdateAppointment(Appointment updated)
        {
            var existing = GetOrThrow(updated.Id);

            ValidateAppointmentTimes(updated);
            ValidateNoConflict(updated, updated.Id);
            ValidateWorkingHours(updated);

            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;
            existing.ServiceId = updated.ServiceId;
            existing.StaffId = updated.StaffId;

            _appointmentRepository.Update(existing);
        }

        public void MarkAsNoShow(int id)
        {
            var appointment = GetOrThrow(id);

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException("Cancelled appointment cannot be NoShow.");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Completed appointment cannot be NoShow.");

            appointment.Status = AppointmentStatus.NoShow;

            _appointmentRepository.Update(appointment);
        }

        // ===================== PRIVATE VALIDATION =====================

        private Appointment GetOrThrow(int id)
        {
            var appointment = _appointmentRepository.GetById(id);

            if (appointment == null)
                throw new InvalidOperationException("Appointment not found.");

            return appointment;
        }

        private void ValidateAppointmentTimes(Appointment a)
        {
            if (a.StartTime >= a.EndTime)
                throw new ArgumentException("Start time must be before end time.");
        }

        private void ValidateNotInPast(Appointment a)
        {
            if (a.StartTime < DateTime.Now)
                throw new InvalidOperationException("Cannot schedule in the past.");
        }

        private void ValidateWorkingHours(Appointment a)
        {
            var schedule = _scheduleRepository
                .GetAll()
                .FirstOrDefault(s =>
                    s.StaffId == a.StaffId &&
                    s.DayOfWeek == a.StartTime.DayOfWeek);

            if (schedule == null)
                throw new InvalidOperationException("Staff is not working that day.");

            if (a.StartTime.TimeOfDay < schedule.StartTime ||
                a.EndTime.TimeOfDay > schedule.EndTime)
            {
                throw new InvalidOperationException("Outside working hours.");
            }
        }

        private void ValidateNoConflict(Appointment a, int? ignoreId = null)
        {
            bool conflict = _appointmentRepository.GetAll().Any(x =>
                x.StaffId == a.StaffId &&
                x.Status != AppointmentStatus.Cancelled &&
                (ignoreId == null || x.Id != ignoreId) &&
                x.StartTime < a.EndTime &&
                a.StartTime < x.EndTime);

            if (conflict)
                throw new InvalidOperationException("Time slot already booked.");
        }
    }
}