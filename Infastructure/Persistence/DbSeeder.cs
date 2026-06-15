using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Staff.Any() || context.Services.Any())
                return;

            var staff1 = new Staff("Ivan Petrov", "Doctor");
            var staff2 = new Staff("Maria Georgieva", "Doctor");

            context.Staff.AddRange(staff1, staff2);
            context.SaveChanges();
      
            var service1 = new Service { Name = "Consultation", Price = 50, DurationInMinutes = 30 };
            var service2 = new Service { Name = "Therapy", Price = 100, DurationInMinutes = 60 };

            context.Services.AddRange(service1, service2);
            context.SaveChanges();
          
            context.Schedules.AddRange(
                new Schedule
                {
                    StaffId = staff1.Id,
                    DayOfWeek = DayOfWeek.Monday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0)
                },
                new Schedule
                {
                    StaffId = staff1.Id,
                    DayOfWeek = DayOfWeek.Tuesday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0)
                },
                new Schedule
                {
                    StaffId = staff2.Id,
                    DayOfWeek = DayOfWeek.Monday,
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0)
                }
            );

            context.SaveChanges();
        }
    }
}