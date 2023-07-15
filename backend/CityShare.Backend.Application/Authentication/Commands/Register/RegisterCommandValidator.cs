using FluentValidation;

namespace CityShare.Backend.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid email address.");

        RuleFor(x => x.Request.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("UserName should contain only letters and digits.");

        RuleFor(x => x.Request.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character."); ;
    }
}
