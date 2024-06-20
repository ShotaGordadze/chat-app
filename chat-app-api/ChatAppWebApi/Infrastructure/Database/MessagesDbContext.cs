using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

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

        var messageEntity = modelBuilder.Entity<Message>();

        messageEntity.ToTable("Messages")
            .HasKey(x => x.Id);
    }

    public DbSet<Message> Messages { get; set; }
}
