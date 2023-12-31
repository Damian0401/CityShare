﻿using CityShare.Backend.Application.Core.Dtos.Images;
using CityShare.Backend.Application.Core.Dtos.Maps;

namespace CityShare.Backend.Application.Core.Dtos.Events;

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public AddressDto Address { get; set; } = default!;
    public int CityId { get; set; }
    public IEnumerable<int> CategoryIds { get; set; } = default!;
    public IEnumerable<ImageDto> Images { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public string Author { get; set; } = default!;
    public int Comments { get; set; }
    public bool IsLiked { get; set; }
}
