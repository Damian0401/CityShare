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
<p><a href=""{EmailPlaceholders.ClientUrl}/confirm-email?id={EmailPlaceholders.Id}&token={EmailPlaceholders.Token}"">Verify email</a></p>
<p><strong>Important:</strong> You need to log in to your account before you can use the link above.</p>
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
        var emailPriorities = new List<EmailPriority>
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
            .Select(x => x.Name)
            .ToList();

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
    
    internal static async Task SeedEmailStatusesAsync(CityShareDbContext context)
    {
        var emailStatuses = typeof(EmailStatuses)
            .GetFields()
            .Select(x => x.GetValue(null))
            .Cast<string>();

        var existingEmailStatuses = context.EmailStatuses
            .AsNoTracking()
            .Select(x => x.Name)
            .ToList();

        foreach (var emailStatus in emailStatuses)
        {
            var statusExists = existingEmailStatuses.Contains(emailStatus);

            if (statusExists) 
            { 
                continue; 
            }

            var newStatus = new EmailStatus
            {
                Name = emailStatus
            };

            context.EmailStatuses.Add(newStatus);
        }

        await context.SaveChangesAsync();
    }
}
