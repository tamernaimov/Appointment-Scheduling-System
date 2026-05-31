using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Domain.Enums;

namespace Appointment_Scheduling_System.Application.Services
{
    public class ReportService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceRepository _serviceRepository;

        public ReportService(
            IAppointmentRepository appointmentRepository,
            IServiceRepository serviceRepository)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _appointmentRepository.GetAll()
                .Where(a => a.StartTime.Date == date.Date)
                .ToList();
        }
        public List<Appointment> GetAppointmentsByStatus(AppointmentStatus status)
        {
            return _appointmentRepository.GetAll()
                .Where(a => a.Status == status)
                .ToList();
        }

        public int GetTotalAppointments()
        {
            return _appointmentRepository.GetAll().Count;
        }

        public int GetCompletedAppointments()
        {
            return _appointmentRepository.GetAll()
                .Count(a => a.Status == AppointmentStatus.Completed);
        }

        public List<Appointment> GetClientHistory(int clientId)
        {
            return _appointmentRepository.GetAll()
                .Where(a => a.ClientId == clientId)
                .OrderByDescending(a => a.StartTime)
                .ToList();
        }

        public decimal GetRevenue(DateTime start, DateTime end)
        {
            var appointments = _appointmentRepository.GetAll()
                .Where(a =>
                    a.Status == AppointmentStatus.Completed &&
                    a.StartTime >= start &&
                    a.StartTime <= end)
                .ToList();

            decimal revenue = 0;

            foreach (var a in appointments)
            {
                var service = _serviceRepository.GetById(a.ServiceId);

                if (service != null)
                    revenue += service.Price;
            }

            return revenue;
        }

        public ReportStats GetStats()
        {
            var all = _appointmentRepository.GetAll();

            return new ReportStats
            {
                Cancelled = all.Count(a => a.Status == AppointmentStatus.Cancelled),
                NoShow = all.Count(a => a.Status == AppointmentStatus.NoShow)
            };
        }
    }
}