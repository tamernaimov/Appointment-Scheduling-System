using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Data;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly JsonDataContext _context;

        public ScheduleRepository(JsonDataContext context)
        {
            _context = context;

        }

        public List<Schedule> GetAll()
        {
            return _context.Schedules;
        }

        public void Add(Schedule schedule)
        {
            var existing = _context.Schedules.FirstOrDefault(s =>
                s.StaffId == schedule.StaffId &&
                s.DayOfWeek == schedule.DayOfWeek);

            if (existing != null)
            {
                existing.StartTime = schedule.StartTime;
                existing.EndTime = schedule.EndTime;

                _context.SaveChanges();
                return;
            }

            schedule.Id = _context.Schedules.Count + 1;

            _context.Schedules.Add(schedule);

            _context.SaveChanges();
        }
    }
}