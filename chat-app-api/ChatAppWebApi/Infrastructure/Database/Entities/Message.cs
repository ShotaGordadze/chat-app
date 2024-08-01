
using Infrastructure.Database.Abstraction;

namespace Infrastructure.Database.Entities;

public class Message : Entity
{
    public string? Username { get; set; }

    public string Context { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
