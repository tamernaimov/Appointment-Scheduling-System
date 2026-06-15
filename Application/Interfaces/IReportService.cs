using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Domain.Enums;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IReportService
    {
        List<Appointment> GetAppointmentsByDate(DateTime date);

        List<Appointment> GetAppointmentsByStatus(AppointmentStatus status);

        int GetTotalAppointments();

        int GetCompletedAppointments();

        List<Appointment> GetClientHistory(int clientId);

        decimal GetRevenue(DateTime start, DateTime end);

        ReportStats GetStats();

        int GetStaffWorkload(int staffId, DateTime start, DateTime end);

        List<(int ServiceId, int Count)> GetMostBookedServices();

        List<Appointment> GetDailySchedule(int staffId, DateTime date);

        List<Appointment> GetWeeklySchedule(int serviceId, DateTime weekStart);
    }
}