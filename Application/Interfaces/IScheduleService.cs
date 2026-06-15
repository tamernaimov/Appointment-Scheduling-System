using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IScheduleService
    {
        void AddSchedule(
            int staffId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime);

        List<Schedule> GetAllSchedules();

        List<Schedule> GetSchedulesForStaff(int staffId);
        void AddScheduleRange(
            int staffId,
            int startDay,
            int endDay,
            TimeSpan startTime,
            TimeSpan endTime);
    }
}