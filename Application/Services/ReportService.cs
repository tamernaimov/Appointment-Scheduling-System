using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Domain.Enums;

namespace Appointment_Scheduling_System.Application.Services
{
    public class ReportService
    {
        private readonly IAppointmentRepository _repository;

        public ReportService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _repository.GetAll()
                .Where(a => a.StartTime.Date == date.Date)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByStatus(AppointmentStatus status)
        {
            return _repository.GetAll()
                .Where(a => a.Status == status)
                .ToList();
        }

        public int GetTotalAppointments()
        {
            return _repository.GetAll().Count;
        }

        public int GetCompletedAppointments()
        {
            return _repository.GetAll()
                .Count(a => a.Status == AppointmentStatus.Completed);
        }
    }
}