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
            if (appointment.StartTime >= appointment.EndTime)
            {
                throw new Exception("Start time must be before end time.");
            }
            if (appointment.StartTime < DateTime.Now)
            {
                throw new Exception("Appointment cannot be in the past.");
            }
            var schedule = _scheduleRepository
                .GetAll()
                .FirstOrDefault(s =>
                    s.StaffId == appointment.StaffId &&
                    s.DayOfWeek == appointment.StartTime.DayOfWeek);
            
            if (schedule == null)
            {
                throw new Exception("Staff is not working on this day.");
            }

            if (appointment.StartTime.TimeOfDay < schedule.StartTime ||
                appointment.EndTime.TimeOfDay > schedule.EndTime)
            {
                throw new Exception("Appointment is outside working hours.");
            }

            bool hasConflict = _appointmentRepository
                .GetAll()
                .Any(a =>
                    a.StaffId == appointment.StaffId &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.StartTime < appointment.EndTime &&
                    appointment.StartTime < a.EndTime);

            if (hasConflict)
            {
                throw new Exception("Time slot is already booked.");
            }

            appointment.Status = AppointmentStatus.Scheduled;

            _appointmentRepository.Add(appointment);
        }
        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
        }

        public void CancelAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);

            if (appointment == null)
                return;

            if ((appointment.StartTime - DateTime.Now).TotalHours < 24)
            {
                throw new Exception(
                    "Appointments can only be cancelled at least 24 hours in advance.");
            }

            appointment.Status = AppointmentStatus.Cancelled;

            _appointmentRepository.Update(appointment);
        }

        public void CompleteAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);

            if (appointment == null)
                return;

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new Exception("Cancelled appointments cannot be completed.");
            }

            appointment.Status = AppointmentStatus.Completed;

            _appointmentRepository.Update(appointment);
        }
        public void UpdateAppointment(Appointment updatedAppointment)
        {
            var existing = _appointmentRepository.GetById(updatedAppointment.Id);

            if (existing == null)
                throw new Exception("Appointment not found.");

            bool hasConflict = _appointmentRepository
                .GetAll()
                .Any(a =>
                    a.Id != updatedAppointment.Id &&
                    a.StaffId == updatedAppointment.StaffId &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.StartTime < updatedAppointment.EndTime &&
                    updatedAppointment.StartTime < a.EndTime);

            if (hasConflict)
            {
                throw new Exception("Time slot is already booked.");
            }

            _appointmentRepository.Update(updatedAppointment);
        }
        public void MarkAsNoShow(int id)
        {
            var appointment = _appointmentRepository.GetById(id);

            if (appointment == null)
                return;

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new Exception("Cancelled appointments cannot be marked as NoShow.");
                //may need a return
            }

            appointment.Status = AppointmentStatus.NoShow;

            _appointmentRepository.Update(appointment);
        }
    }
}