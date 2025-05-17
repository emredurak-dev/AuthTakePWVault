using AuthTakePWVault.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthTakePWVault.Data.Context
{
    public class AuthTakePWVaultContext : DbContext
    {
        public AuthTakePWVaultContext(DbContextOptions<AuthTakePWVaultContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PasswordVault> PasswordVaults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<PasswordVault>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SiteName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired();
                entity.HasOne(d => d.User)
                    .WithMany(p => p.PasswordVaults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
} 