using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }

    public string Lastname { get; set; }

    public DateTime AccCreateTime { get; set; } = DateTime.Now;
}
