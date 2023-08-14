using FluentValidation;

namespace CityShare.Backend.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256)
            .WithName(x => nameof(x.Request.Email));

        RuleFor(x => x.Request.UserName)
            .NotEmpty()
            .MaximumLength(256)
            .MinimumLength(6)
            .Matches("^[a-zA-Z0-9]+$").WithMessage(x => $"'{nameof(x.Request.UserName)}' should contain only letters and digits.")
            .WithName(x => nameof(x.Request.UserName));

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .MinimumLength(6)
            .Matches("[A-Z]").WithMessage(x => $"'{nameof(x.Request.Password)}' must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage(x => $"'{nameof(x.Request.Password)}' must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage(x => $"'{nameof(x.Request.Password)}' must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage(x => $"'{nameof(x.Request.Password)}' must contain at least one non-alphanumeric character.")
            .WithName(x => nameof(x.Request.Password));
            
    }
}
