using Microsoft.AspNetCore.Identity;

namespace ProductApp.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
