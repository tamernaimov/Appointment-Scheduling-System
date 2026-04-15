using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IClientRepository
    {
        void Add(Client client);

        List<Client> GetAll();

        Client GetById(int id);

        void Update(Client client);
    }
}