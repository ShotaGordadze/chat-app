using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }

    public string Lastname { get; set; }

    public DateTime AccCreateDate { get; set; }

    public List<User> Friends { get; set; }
}
