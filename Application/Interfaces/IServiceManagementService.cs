using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IServiceManagementService
    {
        void AddService(string name, int duration, decimal price);

        List<Service> GetAllServices();

        Service GetService(int id);
    }
}