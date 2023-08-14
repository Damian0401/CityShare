using FluentValidation;

namespace CityShare.Backend.Application.Emails.Commands.SendNewEmail;

public class SendNewEmailCommandValidator : AbstractValidator<SendNewEmailCommand>
{
    public SendNewEmailCommandValidator()
    {
        RuleFor(x => x.EmailId)
            .NotEmpty();
    }
}
