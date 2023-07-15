using FluentValidation;

namespace CityShare.Backend.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(x => x.Request.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Email is not valid email address.");

		RuleFor(x => x.Request.Password)
			.NotEmpty().WithMessage("Password is required.");
	}
}
