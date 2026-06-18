using FitnessSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<GroupClass> GroupClasses { get; set; }
        public DbSet<GroupClassBooking> GroupClassBookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Phone)
                .IsUnique();
        }
    }
}
