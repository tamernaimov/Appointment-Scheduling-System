using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public void AddStaff(string name, string position)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Staff name is required.");

            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("Position is required.");

            var staff = new Staff
            {
                Name = name.Trim(),
                Position = position.Trim()
            };

            _staffRepository.Add(staff);
        }

        public List<Staff> GetAllStaff()
        {
            return _staffRepository.GetAll();
        }

        public Staff GetStaff(int id)
        {
            return _staffRepository.GetById(id);
        }
    }
}