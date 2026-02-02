using Microsoft.EntityFrameworkCore;
using TravelDeskAPI.Models;

namespace TravelDeskAPI.Data
{
    public class TravelDeskDbContext : DbContext
    {
        public TravelDeskDbContext(DbContextOptions<TravelDeskDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TravelRequest> TravelRequests { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.EmployeeID)
                .IsUnique();

            modelBuilder.Entity<TravelRequest>()
                .HasKey(tr => tr.Id);

            modelBuilder.Entity<TravelRequest>()
                .HasIndex(tr => tr.RequestNumber)
                .IsUnique();

            modelBuilder.Entity<TravelRequest>()
                .HasOne(tr => tr.Employee)
                .WithMany(u => u.TravelRequests)
                .HasForeignKey(tr => tr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TravelRequest>()
                .HasOne(tr => tr.Manager)
                .WithMany()
                .HasForeignKey(tr => tr.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Document>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.TravelRequest)
                .WithMany(tr => tr.Documents)
                .HasForeignKey(d => d.TravelRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.TravelRequest)
                .WithMany(tr => tr.Comments)
                .HasForeignKey(c => c.TravelRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
