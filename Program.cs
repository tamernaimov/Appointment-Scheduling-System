using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Appointment_Scheduling_System.Infrastructure.Persistence;
using Appointment_Scheduling_System.Infrastructure.Repositories;
using Appointment_Scheduling_System.Application.Services;
using Appointment_Scheduling_System.Application.Interfaces;
using Appointment_Scheduling_System.ConsoleUI.Menus;


var services = new ServiceCollection();

var connectionString =
    "Server=.\\SQLEXPRESS;Database=AppointmentDb;Integrated Security=True;TrustServerCertificate=True;";


services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IAppointmentRepository, AppointmentRepository>();
services.AddScoped<IServiceRepository, ServiceRepository>();
services.AddScoped<IStaffRepository, StaffRepository>();
services.AddScoped<IScheduleRepository, ScheduleRepository>();


services.AddScoped<IClientService, ClientService>();
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IScheduleService, ScheduleService>();
services.AddScoped<IReportService, ReportService>();
services.AddScoped<IStaffService, StaffService>();

services.AddScoped<MainMenu>();

var provider = services.BuildServiceProvider();
using (var scope = provider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    DbSeeder.Seed(context);
}

var menu = provider.GetRequiredService<MainMenu>();
menu.Show();