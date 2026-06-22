using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public List<Client> GetAll()
        {
            return _context.Clients.ToList();
        }

        public Client GetById(int id)
        {
            return _context.Clients.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Client client)
        {
            _context.Clients.Update(client);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var client = _context.Clients.FirstOrDefault(x => x.Id == id);
            if (client == null) return;

            _context.Clients.Remove(client);
            _context.SaveChanges();
        }
    }
}