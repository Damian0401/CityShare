using FluentValidation;

namespace CityShare.Backend.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(x => x.Request.Email)
			.NotEmpty()
			.EmailAddress()
			.WithName(x => nameof(x.Request.Email));

		RuleFor(x => x.Request.Password)
			.NotEmpty()
			.WithName(x => nameof(x.Request.Password));
	}
}
