using CityShare.Backend.Application.Core.Dtos.Comments;

namespace CityShare.Backend.Application.Core.Abstractions.Comments;

public interface ICommentClient
{
    Task AddCommentAsync(CommentDto comment);
}
