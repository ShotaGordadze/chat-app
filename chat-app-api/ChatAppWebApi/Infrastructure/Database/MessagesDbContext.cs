using Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Database;

public class MessagesDbContext : DbContext
{
    public MessagesDbContext()
    {
    }

    public MessagesDbContext(DbContextOptions<MessagesDbContext> dbContext)
        : base(dbContext)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=ChatApp_db;Pooling=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users", "identity");
        modelBuilder.Entity<Role>().ToTable("Roles", "identity");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", "identity");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", "identity");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", "identity");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", "identity");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", "identity");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasKey(r => new { r.UserId, r.RoleId });

        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .HasKey(rc => rc.Id);

        var messageEntity = modelBuilder.Entity<Message>();

        messageEntity.ToTable("Messages")
            .HasKey(x => x.Id);
    }

    public DbSet<Message> Messages { get; set; }
}
