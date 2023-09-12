namespace CityShare.Backend.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string AuthorId { get; set; } = default!;
    public int CityId { get; set; }
    public int AddressId { get; set; }

    public virtual ApplicationUser Author { get; set; } = default!;
    public virtual City City { get; set; } = default!;
    public virtual Address Address { get; set; } = default!;
    public virtual IEnumerable<Like> Likes { get; set; } = default!;
    public virtual IEnumerable<Image> Images { get; set; } = default!;
    public virtual IEnumerable<Comment> Comments { get; set; } = default!;
}
