using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IScheduleRepository
    {
        void Add(Schedule schedule);

        List<Schedule> GetAll();

        Schedule GetById(int id);

        void Update(Schedule schedule);

        void Delete(int id);
    }
}