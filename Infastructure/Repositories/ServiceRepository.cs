using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Data;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class ServiceRepository
    {
        private readonly JsonDataContext _context;

        public ServiceRepository(JsonDataContext context)
        {
            _context = context;
        }

        public void Add(Service service)
        {
            service.Id = _context.Services.Count + 1;
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public List<Service> GetAll()
        {
            return _context.Services;
        }

        public Service GetById(int id)
        {
            return _context.Services.FirstOrDefault(s => s.Id == id);
        }
    }
}
