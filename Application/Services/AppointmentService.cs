
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

        // ================= CREATE =================
        public void CreateAppointment(int clientId, int staffId, int serviceId, DateTime start, DateTime end)
        {
            ValidateNotInPast(start);
            ValidateTimeRange(start, end);

            var schedule = GetSchedule(staffId, start);

            ValidateWorkingHours(schedule, start, end);
            ValidateNoConflict(staffId, start, end, null);

            var appointment = new Appointment(clientId, staffId, serviceId, start, end);

            _appointmentRepository.Add(appointment);
        }

        // ================= READ =================
        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
        }

        // ================= CANCEL =================
        public void CancelAppointment(int id)
        {
            var appointment = GetOrThrow(id);

            appointment.Cancel();

            _appointmentRepository.Update(appointment);
        }

        // ================= COMPLETE =================
        public void CompleteAppointment(int id)
        {
            var appointment = GetOrThrow(id);

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException("Cancelled appointment cannot be completed.");

            appointment.Complete();

            _appointmentRepository.Update(appointment);
        }

        // ================= UPDATE =================
        public void UpdateAppointment(Appointment updated)
        {
            var existing = GetOrThrow(updated.Id);

            ValidateTimeRange(updated.StartTime, updated.EndTime);
            ValidateNotInPast(updated.StartTime);

            var schedule = GetSchedule(updated.StaffId, updated.StartTime);

            ValidateWorkingHours(schedule, updated.StartTime, updated.EndTime);
            ValidateNoConflict(updated.StaffId, updated.StartTime, updated.EndTime, updated.Id);

            existing.Reschedule(updated.StartTime, updated.EndTime);

            _appointmentRepository.Update(existing);
        }

        // ================= NO SHOW =================
        public void MarkAsNoShow(int id)
        {
            var appointment = GetOrThrow(id);

            appointment.MarkNoShow();

            _appointmentRepository.Update(appointment);
        }

        // ================= DELETE =================
        public void DeleteAppointment(int id)
        {
            GetOrThrow(id);

            _appointmentRepository.Delete(id);
        }

        // ================= HELPERS =================

        private Appointment GetOrThrow(int id)
        {
            var appointment = _appointmentRepository.GetById(id);

            if (appointment == null)
                throw new InvalidOperationException("Appointment not found.");

            return appointment;
        }

        private void ValidateTimeRange(DateTime start, DateTime end)
        {
            if (start >= end)
                throw new ArgumentException("Start time must be before end time.");
        }

        private void ValidateNotInPast(DateTime start)
        {
            if (start < DateTime.Now)
                throw new InvalidOperationException("Cannot schedule in the past.");
        }

        private Schedule GetSchedule(int staffId, DateTime date)
        {
            return _scheduleRepository.GetAll()
                .FirstOrDefault(s =>
                    s.StaffId == staffId &&
                    s.DayOfWeek == date.DayOfWeek);
        }

        private void ValidateWorkingHours(Schedule schedule, DateTime start, DateTime end)
        {
            if (schedule == null)
                throw new InvalidOperationException("Staff is not working that day.");

            if (start.TimeOfDay < schedule.StartTime ||
                end.TimeOfDay > schedule.EndTime)
            {
                throw new InvalidOperationException("Outside working hours.");
            }
        }

        private void ValidateNoConflict(int staffId, DateTime start, DateTime end, int? ignoreId)
        {
            bool conflict = _appointmentRepository.GetAll().Any(x =>
                x.StaffId == staffId &&
                x.Status != AppointmentStatus.Cancelled &&
                (ignoreId == null || x.Id != ignoreId) &&
                x.StartTime < end &&
                start < x.EndTime);

            if (conflict)
                throw new InvalidOperationException("Time slot already booked.");
        }
    }
}