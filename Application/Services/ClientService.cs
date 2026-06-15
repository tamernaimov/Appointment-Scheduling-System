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

        public void CreateClient(string firstName, string lastName, string phone, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.");

            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone number is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            var client = new Client
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phone,
                Email = email
            };

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
