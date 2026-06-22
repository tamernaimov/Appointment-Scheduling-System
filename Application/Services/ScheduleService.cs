using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Services
{
    public class ScheduleService : IScheduleService
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

            var schedule = new Schedule(staffId, dayOfWeek, startTime, endTime);

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
        public void AddScheduleRange(
    int staffId,
    int startDay,
    int endDay,
    TimeSpan startTime,
    TimeSpan endTime)
        {
            if (startDay < 1 || startDay > 7 || endDay < 1 || endDay > 7)
                throw new ArgumentException("Days must be between 1 and 7.");

            if (startDay > endDay)
                throw new ArgumentException("Start day cannot be after end day.");

            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time.");

            for (int day = startDay; day <= endDay; day++)
            {
                DayOfWeek dayOfWeek = (DayOfWeek)(day % 7);

                // fix Sunday mapping (because enum starts at Sunday = 0)
                if (day == 7)
                    dayOfWeek = DayOfWeek.Sunday;

                var schedule = new Schedule(staffId, dayOfWeek, startTime, endTime);

                _scheduleRepository.Add(schedule);
            }
        }
    }
}