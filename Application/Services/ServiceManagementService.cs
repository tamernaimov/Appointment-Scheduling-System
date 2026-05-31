using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Services
{
    public class ServiceManagementService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceManagementService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public void AddService(string name, int duration, decimal price)
        {
            var service = new Service
            {
                Name = name,
                DurationInMinutes = duration,
                Price = price
            };

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