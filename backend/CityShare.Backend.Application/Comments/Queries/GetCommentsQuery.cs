using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Comments;
using CityShare.Backend.Application.Core.Dtos.Comments;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Comments.Queries;

public record GetCommentsQuery(Guid EventId) : IRequest<Result<IEnumerable<CommentDto>>>;

public class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
{
    public GetCommentsQueryValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty();
    }
}

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, Result<IEnumerable<CommentDto>>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCommentsQueryHandler> _logger;

    public GetCommentsQueryHandler(
        ICommentRepository commentRepository,
        IMapper mapper,
        ILogger<GetCommentsQueryHandler> logger)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CommentDto>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting comments using {@Type}", _commentRepository.GetType());
        var comments = await _commentRepository.GetCommentsByEventIdAsync(request.EventId, cancellationToken);

        _logger.LogInformation("Mapping comments to dtos");
        var dtos = _mapper.Map<IEnumerable<CommentDto>>(comments);

        return Result<IEnumerable<CommentDto>>.Success(dtos);
    }
}
