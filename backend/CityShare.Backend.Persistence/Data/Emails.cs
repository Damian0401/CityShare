using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityShare.Backend.Persistence.Data;

internal static class Emails
{
    internal async static Task SeedEmailTemplatesAsync(CityShareDbContext context)
    {
        var existingTemplates = context.EmailTemplates
            .AsNoTracking()
            .Select(x => x.Name).ToList();

        var emailTemplates = new List<EmailTemplate>
        {
            new EmailTemplate
            {
                Name = EmailTemplates.WelcomeAndEmailConfirmLink,
                Subject = "Welcome to CityShare!",
                Body = @$"<p>Hi {EmailPlaceholders.UserName},</p>
<p>Thank you for registering at <a href=""{EmailPlaceholders.ClientUrl}"">CityShare</a>.</p>
<p>Please verify your email by clicking the link below.</p>
<p><a href=""{EmailPlaceholders.ClientUrl}/confirm-email?code={EmailPlaceholders.Code}&id={EmailPlaceholders.Id}"">Verify email</a></p>
<p>Best regards,</p>
<p>CityShare team</p>
<p><small>If you did not register at <a href=""{EmailPlaceholders.ClientUrl}"">CityShare</a>, please ignore this email.</small></p>"
            }
        };

        foreach (var template in emailTemplates)
        {
            var templateExists = existingTemplates.Contains(template.Name);

            if (templateExists)
            {
                continue;
            }

            context.EmailTemplates.Add(template);
        }

        await context.SaveChangesAsync();
    }

    internal static async Task SeedEmailPrioritiesAsync(CityShareDbContext context)
    {
        var emailPriorities = new List<EmailPrirority>
        {
            new()
            {
                Name = EmailPriorities.High,
                RetryNumber = 15
            },
            new()
            {
                Name = EmailPriorities.Medium,
                RetryNumber = 5
            },
            new()
            {
                Name = EmailPriorities.Low,
                RetryNumber = 1
            },
        };

        var existingEmailPriorities = context.EmailPriorities
            .AsNoTracking()
            .Select(x => x.Name).ToList();

        foreach (var emailPriority in emailPriorities)
        {
            var priorityExists = existingEmailPriorities.Contains(emailPriority.Name);

            if (priorityExists)
            {
                continue;
            }

            context.EmailPriorities.Add(emailPriority);
        }

        await context.SaveChangesAsync();
    }
}
