namespace CityShare.Backend.Application.Core.Dtos.Comments;

public class CommentDto
{
    public string Message { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string AuthorId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
