using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Application.Interfaces
{
    public interface IStaffRepository
    {
        void Add(Staff staff);

        List<Staff> GetAll();

        Staff GetById(int id);
    }
}