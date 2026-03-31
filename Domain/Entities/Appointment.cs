using Appointment_Scheduling_System.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Scheduling_System.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int StaffId { get; set; }

        public int ServiceId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        //public AppointmentStatus Status { get; set; }
    }
}
