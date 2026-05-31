using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Services
{
    public class ScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public void AddSchedule(
            int staffId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            if (startTime >= endTime)
            {
                throw new Exception("Invalid working hours.");
            }

            var schedule = new Schedule
            {
                StaffId = staffId,
                DayOfWeek = dayOfWeek,
                StartTime = startTime,
                EndTime = endTime
            };

            _scheduleRepository.Add(schedule);
        }

        public List<Schedule> GetAllSchedules()
        {
            return _scheduleRepository.GetAll();
        }

        public List<Schedule> GetSchedulesForStaff(int staffId)
        {
            return _scheduleRepository
                .GetAll()
                .Where(s => s.StaffId == staffId)
                .ToList();
        }
    }
}