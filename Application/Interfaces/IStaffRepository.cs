using Appointment_Scheduling_System.Domain.Entities;

public interface IStaffRepository
{
    void Add(Staff staff);
    List<Staff> GetAll();
    Staff GetById(int id);
    void Delete(int id);
}