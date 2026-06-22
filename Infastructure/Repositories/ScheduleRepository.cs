using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Persistence;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext _context;

        public ScheduleRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            _context.SaveChanges();
        }

        public List<Schedule> GetAll()
        {
            return _context.Schedules.ToList();
        }

        public void Delete(int id)
        {
            var schedule = _context.Schedules.FirstOrDefault(x => x.Id == id);
            if (schedule == null) return;

            _context.Schedules.Remove(schedule);
            _context.SaveChanges();
        }
    }
}