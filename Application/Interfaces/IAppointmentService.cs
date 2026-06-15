using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IAppointmentService
    {
        void CreateAppointment(int clientId, int staffId, int serviceId, DateTime start, DateTime end);

        List<Appointment> GetAll();

        void CancelAppointment(int id);

        void CompleteAppointment(int id);

        void UpdateAppointment(Appointment updatedAppointment);

        void MarkAsNoShow(int id);
    }
}