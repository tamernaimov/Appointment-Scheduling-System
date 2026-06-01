using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IScheduleRepository
    {
        List<Schedule> GetAll();
        void Add(Schedule schedule);
    }
}