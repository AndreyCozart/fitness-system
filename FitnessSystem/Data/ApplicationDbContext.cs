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

            // Настройка уникальности телефона
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Phone)
                .IsUnique();

            // Настройка уникальности имени пользователя
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Настройка связей
            modelBuilder.Entity<Membership>()
                .HasOne(m => m.Client)
                .WithMany(c => c.Memberships)
                .HasForeignKey(m => m.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Membership>()
                .HasOne(m => m.Type)
                .WithMany(t => t.Memberships)
                .HasForeignKey(m => m.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Client)
                .WithMany(c => c.Visits)
                .HasForeignKey(v => v.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Membership)
                .WithMany(m => m.Visits)
                .HasForeignKey(v => v.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupClassBooking>()
                .HasOne(b => b.GroupClass)
                .WithMany(g => g.Bookings)
                .HasForeignKey(b => b.GroupClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupClassBooking>()
                .HasOne(b => b.Client)
                .WithMany()
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Добавляем тестового администратора
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123", // В реальном проекте нужно хешировать!
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}