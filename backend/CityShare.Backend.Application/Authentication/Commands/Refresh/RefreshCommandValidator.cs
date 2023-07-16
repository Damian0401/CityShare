using FluentValidation;

namespace CityShare.Backend.Application.Authentication.Commands.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(x => x.Request.AccessToken)
            .NotEmpty()
            .WithName(x => nameof(x.Request.AccessToken));

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithName(x => nameof(x.RefreshToken));
    }
}
