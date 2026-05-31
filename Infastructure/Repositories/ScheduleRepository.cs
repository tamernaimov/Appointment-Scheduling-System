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

        public void Add(Schedule schedule)
        {
            schedule.Id = _context.Schedules.Count + 1;

            _context.Schedules.Add(schedule);

            _context.SaveChanges();
        }

        public List<Schedule> GetAll()
        {
            return _context.Schedules;
        }

        public Schedule GetById(int id)
        {
            return _context.Schedules.FirstOrDefault(s => s.Id == id);
        }

        public void Update(Schedule schedule)
        {
            var existing = GetById(schedule.Id);

            if (existing == null)
                return;

            existing.StaffId = schedule.StaffId;
            existing.DayOfWeek = schedule.DayOfWeek;
            existing.StartTime = schedule.StartTime;
            existing.EndTime = schedule.EndTime;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var schedule = GetById(id);

            if (schedule == null)
                return;

            _context.Schedules.Remove(schedule);

            _context.SaveChanges();
        }
    }
}