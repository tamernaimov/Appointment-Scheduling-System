using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void CreateClient(string first, string last, string phone, string email)
        {
            var client = new Client(first, last, phone, email);
            _clientRepository.Add(client);
        }

        public List<Client> GetAllClients()
        {
            return _clientRepository.GetAll();
        }

        public Client GetClient(int id)
        {
            return _clientRepository.GetById(id);
        }

        public void UpdateClient(Client client)
        {
            _clientRepository.Update(client);
        }
    }
}
