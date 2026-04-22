using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Data;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class StaffRepository
    {
        private readonly JsonDataContext _context;

        public StaffRepository(JsonDataContext context)
        {
            _context = context;
        }

        public void Add(Staff staff)
        {
            staff.Id = _context.Staff.Count + 1;
            _context.Staff.Add(staff);
            _context.SaveChanges();
        }

        public List<Staff> GetAll()
        {
            return _context.Staff;
        }

        public Staff GetById(int id)
        {
            return _context.Staff.FirstOrDefault(s => s.Id == id);
        }
    }
}
