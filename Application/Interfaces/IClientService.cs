using Appointment_Scheduling_System.Domain.Entities;

public interface IClientService
{
    void CreateClient(
        string firstName,
        string lastName,
        string phone,
        string email);

    List<Client> GetAllClients();

    Client GetClient(int id);

    void UpdateClient(Client client);
}