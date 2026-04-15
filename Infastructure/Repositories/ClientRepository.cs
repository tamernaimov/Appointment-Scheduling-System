using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Data;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly JsonDataContext _context;

        public ClientRepository(JsonDataContext context)
        {
            _context = context;
        }

        public void Add(Client client)
        {
            client.Id = _context.Clients.Count + 1;
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public List<Client> GetAll()
        {
            return _context.Clients;
        }

        public Client GetById(int id)
        {
            return _context.Clients.FirstOrDefault(c => c.Id == id);
        }

        public void Update(Client client)
        {
            var existing = GetById(client.Id);
            if (existing == null) return;

            existing.FirstName = client.FirstName;
            existing.LastName = client.LastName;
            existing.PhoneNumber = client.PhoneNumber;
            existing.Email = client.Email;

            _context.SaveChanges();
        }
    }
}