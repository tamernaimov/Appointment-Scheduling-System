using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IStaffService
    {
        void AddStaff(string name, string position);

        List<Staff> GetAllStaff();

        Staff GetStaff(int id);
    }
}