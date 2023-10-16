using CityShare.Backend.Application.Core.Abstractions.Comments;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Comments.Commands;

public record AddCommentCommand(
    Guid EventId,
    string UserId,
    string Meesage) 
    : IRequest<Result>;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Meesage)
            .NotEmpty();
    }
}

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IClock _clock;
    private readonly ILogger<AddCommentCommandHandler> _logger;

    public AddCommentCommandHandler(
        ICommentRepository commentRepository,
        IClock clock,
        ILogger<AddCommentCommandHandler> logger)
    {
        _commentRepository = commentRepository;
        _clock = clock;
        _logger = logger;
    }

    public async Task<Result> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating comment");
        var comment = new Comment
        {
            AuthorId = request.UserId,
            Message = request.Meesage,
            CreatedAt = _clock.Now,
            EventId = request.EventId,
        };

        _logger.LogInformation("Adding comment using {@Type}", _commentRepository.GetType());
        await _commentRepository.AddAsync(comment, cancellationToken);

        return Result.Success();
    }
}
