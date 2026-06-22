
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IServiceRepository
    {
        void Add(Service service);
        List<Service> GetAll();
        Service GetById(int id);
        void Delete(int id);
    }
}