
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Persistence;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;

        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Staff staff)
        {
            _context.Staff.Add(staff);
            _context.SaveChanges();
        }

        public List<Staff> GetAll()
        {
            return _context.Staff.ToList();
        }

        public Staff GetById(int id)
        {
            return _context.Staff.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(int id)
        {
            var staff = _context.Staff.FirstOrDefault(x => x.Id == id);
            if (staff == null) return;

            _context.Staff.Remove(staff);
            _context.SaveChanges();
        }
    }
}