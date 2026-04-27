using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IAppointmentRepository
    {
        void Add(Appointment appointment);

        List<Appointment> GetAll();

        Appointment GetById(int id);

        void Update(Appointment appointment);

        void Delete(int id);
    }
}