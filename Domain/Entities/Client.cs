using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Scheduling_System.Domain.Entities
{
    public class Client
    {   
       public int Id { get; set; }

       public string FirstName { get; set; }

       public string LastName { get; set; }

       public string PhoneNumber { get; set; }

       public string Email { get; set; }
        
    }
}
