using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Dtos.Events;

public class SearchEventDto
{
    public Event Event { get; set; } = default!;
    public int Likes { get; set; }
    public int CommentNumber { get; set; }
}
