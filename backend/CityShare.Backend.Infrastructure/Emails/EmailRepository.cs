using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Models.Emails;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Infrastructure.Emails;

public class EmailRepository : IEmailRepository
{
    private readonly CityShareDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<EmailRepository> _logger;

    public EmailRepository(
        CityShareDbContext context,
        IMapper mapper,
        ILogger<EmailRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(CreateEmailModel model)
    {
        _logger.LogInformation("Searching for template with name: {@Name}", model.Template);
        var template = await _context.EmailTemplates
            .AsNoTracking()
            .FirstAsync(x => x.Name.Equals(model.Template));

        _logger.LogInformation("Searching for priority with name: {@Name}", model.Priority);
        var emailPrirority = await _context.EmailPriorities
            .AsNoTracking()
            .FirstAsync(x => x.Name.Equals(model.Priority));

        _logger.LogInformation("Creating email from model {@Model}", model);
        var email = CreateEmail(model, template, emailPrirority);

        _logger.LogInformation("Saving email to database {@Email}", email);
        _context.Emails.Add(email);
        await _context.SaveChangesAsync();

        return email.Id;
    }

    public async Task<Email?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting email by id {@Id}", id);
        var email = await _context.Emails
            .AsNoTracking()
            .FirstAsync(x => x.Id.Equals(id));

        return email;
    }

    private Email CreateEmail(CreateEmailModel model, EmailTemplate template, EmailPrirority emailPrirority)
    {
        var email = _mapper.Map<Email>(template);
        email.EmailPrirorityId = emailPrirority.Id;
        email.Receiver = model.Receiver;
        email.Status = EmailStatuses.New;

        foreach (var (key, value) in model.Parameters)
        {
            email.Subject = email.Subject.Replace(key, value);
            email.Body = email.Body.Replace(key, value);
        }

        return email;
    }
}
