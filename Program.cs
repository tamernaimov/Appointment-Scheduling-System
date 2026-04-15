using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Data;
using Appointment_Scheduling_System.Infrastructure.Persistence;
using Appointment_Scheduling_System.Infrastructure.Repositories;

namespace Appointment_Scheduling_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileService = new JsonFileService();
            var context = new JsonDataContext(fileService);
            var clientRepo = new ClientRepository(context);

            clientRepo.Add(new Client
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                PhoneNumber = "0888",
                Email = "ivan@test.com"
            });
        }
    }
}
