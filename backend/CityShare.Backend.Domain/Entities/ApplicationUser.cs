using Microsoft.AspNetCore.Identity;

namespace CityShare.Backend.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public virtual IEnumerable<Event> Events { get; set; } = default!;
    public virtual IEnumerable<Like> Likes { get; set; } = default!;
    public virtual IEnumerable<Comment> Comments { get; set; } = default!;
    public virtual IEnumerable<Request> Requests { get; set; } = default!;
}
