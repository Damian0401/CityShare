using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Comments;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Application.Core.Dtos.Comments;
using CityShare.Backend.Application.Core.Hubs;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Comments.Commands;

public record AddCommentCommand(
    Guid EventId,
    string UserId,
    string UserName,
    CreateCommentDto Dto) 
    : IRequest<Result>;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Dto.Message)
            .NotEmpty()
            .WithName(x => nameof(x.Dto.Message));
    }
}

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IClock _clock;
    private readonly IHubContext<CommentHub, ICommentClient> _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AddCommentCommandHandler> _logger;

    public AddCommentCommandHandler(
        ICommentRepository commentRepository,
        IClock clock,
        IHubContext<CommentHub, ICommentClient> context,
        IMapper mapper,
        ILogger<AddCommentCommandHandler> logger)
    {
        _commentRepository = commentRepository;
        _clock = clock;
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating comment");
        var comment = new Comment
        {
            AuthorId = request.UserId,
            Message = request.Dto.Message,
            CreatedAt = _clock.Now,
            EventId = request.EventId,
        };

        _logger.LogInformation("Adding comment using {@Type}", _commentRepository.GetType());
        await _commentRepository.AddAsync(comment, cancellationToken);

        _logger.LogInformation("Creating dto");
        var dto = _mapper.Map<CommentDto>(comment);
        dto.Author = request.UserName;

        _logger.LogInformation("Invoking {@Name} using {@Type}", nameof(ICommentClient.AddCommentAsync), _context.GetType());
        await _context.Clients
            .Group(request.EventId.ToString())
            .AddCommentAsync(dto);

        return Result.Success();
    }
}
