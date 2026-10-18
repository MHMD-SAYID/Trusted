using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trusted.Core.Entities;
using Trusted.Core.Entities.Games;

namespace Trusted.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<User, UserRole,long>
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Image> Images { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasIndex(u=>u.PhoneNumber)
            .IsUnique();
        builder.Entity<Game>()
            .HasIndex(c => c.EnName)
            .IsUnique();
        builder.Entity<Game>()
            .HasIndex(c => c.ArName)
            .IsUnique();

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
