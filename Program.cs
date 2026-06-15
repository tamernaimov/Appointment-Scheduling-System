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

// ================= REPOSITORIES =================
services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IAppointmentRepository, AppointmentRepository>();
services.AddScoped<IServiceRepository, ServiceRepository>();
services.AddScoped<IStaffRepository, StaffRepository>();
services.AddScoped<IScheduleRepository, ScheduleRepository>();

// ================= SERVICES =================
services.AddScoped<IClientService, ClientService>();
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IScheduleService, ScheduleService>();
services.AddScoped<IReportService, ReportService>();
services.AddScoped<IStaffService, StaffService>();

// ================= MENUS =================
services.AddScoped<MainMenu>();
services.AddScoped<ClientMenu>();
services.AddScoped<StaffMenu>();
services.AddScoped<ServiceMenu>();
services.AddScoped<AppointmentMenu>();
services.AddScoped<ReportMenu>();
services.AddScoped<ScheduleMenu>();

var provider = services.BuildServiceProvider();

using (var scope = provider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    DbSeeder.Seed(context);
}

var menu = provider.GetRequiredService<MainMenu>();
menu.Show();