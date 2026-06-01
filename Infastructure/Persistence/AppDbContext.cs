using Microsoft.EntityFrameworkCore;
using Appointment_Scheduling_System.Domain.Entities;

namespace Appointment_Scheduling_System.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .HasOne<Client>()
                .WithMany()
                .HasForeignKey(x => x.ClientId);

            modelBuilder.Entity<Appointment>()
                .HasOne<Staff>()
                .WithMany()
                .HasForeignKey(x => x.StaffId);

            modelBuilder.Entity<Appointment>()
                .HasOne<Service>()
                .WithMany()
                .HasForeignKey(x => x.ServiceId);
        }
    }
}