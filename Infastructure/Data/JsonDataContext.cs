using Appointment_Scheduling_System.Domain.Entities;
using Appointment_Scheduling_System.Infrastructure.Persistence;

namespace Appointment_Scheduling_System.Infrastructure.Data
{
    public class JsonDataContext
    {
        private readonly JsonFileService _fileService;
        public List<Client> Clients { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Service> Services { get; set; }
        public List<Staff> Staff { get; set; }
        public List<Schedule> Schedules { get; set; }

        private readonly string clientsFile = "../../../Data/clients.json";
        private readonly string appointmentsFile = "../../../Data/appointments.json";
        private readonly string servicesFile = "../../../Data/services.json";
        private readonly string staffFile = "../../../Data/staff.json";
        private readonly string schedulesFile = "../../../Data/schedules.json";


        public JsonDataContext(JsonFileService fileService)
        {
            _fileService = fileService;

            Clients = _fileService.Read<Client>(clientsFile);
            Appointments = _fileService.Read<Appointment>(appointmentsFile);
            Services = _fileService.Read<Service>(servicesFile);
            Staff = _fileService.Read<Staff>(staffFile);
            Schedules = _fileService.Read<Schedule>(schedulesFile);
        }

        public void SaveChanges()
        {
            _fileService.Write(clientsFile, Clients);
            _fileService.Write(appointmentsFile, Appointments);
            _fileService.Write(servicesFile, Services);
            _fileService.Write(staffFile, Staff);
            _fileService.Write(schedulesFile, Schedules);
        }
    }
}
