using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IClientService
    {
        void CreateClient(string first, string last, string phone, string email);
        List<Client> GetAllClients();
        Client GetClient(int id);
        void UpdateClient(Client client);
        void DeleteClient(int id);
    }
}