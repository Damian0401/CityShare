using FluentValidation;

namespace CityShare.Backend.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .NotEmpty()
            .WithName(x => nameof(x.Request.Id));

        RuleFor(x => x.Request.Token)
            .NotEmpty()
            .WithName(x => x.Request.Token);
    }
}
