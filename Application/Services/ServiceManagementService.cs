using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Services
{
    public class ServiceManagementService : IServiceManagementService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceManagementService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public void AddService(string name, int duration, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            if (duration <= 0)
                throw new ArgumentException("Duration must be positive.");

            if (price < 0)
                throw new ArgumentException("Price cannot be negative.");

            var service = new Service(name, duration, price);

            _serviceRepository.Add(service);
        }

        public List<Service> GetAllServices()
        {
            return _serviceRepository.GetAll();
        }

        public Service GetService(int id)
        {
            return _serviceRepository.GetById(id);
        }
    }
}