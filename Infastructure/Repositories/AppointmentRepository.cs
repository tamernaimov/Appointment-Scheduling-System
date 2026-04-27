using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Data;

namespace Appointment_Scheduling_System.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly JsonDataContext _context;

        public AppointmentRepository(JsonDataContext context)
        {
            _context = context;
        }

        public void Add(Appointment appointment)
        {
            appointment.Id = _context.Appointments.Count + 1;
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments;
        }

        public Appointment GetById(int id)
        {
            return _context.Appointments.FirstOrDefault(a => a.Id == id);
        }

        public void Update(Appointment appointment)
        {
            var existing = GetById(appointment.Id);
            if (existing == null) return;

            existing.StartTime = appointment.StartTime;
            existing.EndTime = appointment.EndTime;
            existing.Status = appointment.Status;
            existing.ClientId = appointment.ClientId;
            existing.StaffId = appointment.StaffId;
            existing.ServiceId = appointment.ServiceId;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var appointment = GetById(id);
            if (appointment == null) return;

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
        }
    }
}
