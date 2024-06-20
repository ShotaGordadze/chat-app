
namespace Infrastructure.Database.Entities;

public class Message
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Context { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
