using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Persistence;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public List<Service> GetAll()
        {
            return _context.Services.ToList();
        }

        public Service GetById(int id)
        {
            return _context.Services.FirstOrDefault(x => x.Id == id);
        }
    }
}