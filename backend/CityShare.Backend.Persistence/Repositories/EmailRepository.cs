using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Dtos.Emails.Create;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

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

    public async Task<Guid> CreateAsync(CreateEmailDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for template with name: {@Name}", dto.Template);
        var template = await _context.EmailTemplates
            .AsNoTracking()
            .FirstAsync(x => x.Name.Equals(dto.Template), cancellationToken);

        _logger.LogInformation("Searching for priority with name: {@Name}", dto.Priority);
        var emailPriority = await _context.EmailPriorities
            .AsNoTracking()
            .FirstAsync(x => x.Name.Equals(dto.Priority), cancellationToken);

        _logger.LogInformation("Creating email from dto {@Dto}", dto);
        var email = await CreateEmailAsync(dto, template, emailPriority);

        _logger.LogInformation("Saving new email to database");
        _context.Emails.Add(email);
        await _context.SaveChangesAsync(cancellationToken);

        return email.Id;
    }

    public async Task<Email?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting email by id {@Id}", id);
        var email = await _context.Emails
            .AsNoTracking()
            .FirstAsync(x => x.Id.Equals(id), cancellationToken);

        return email;
    }

    public async Task<bool> UpdateAsync(Email email, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating email with id {@Id}", email.Id);
        _context.Emails.Update(email);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> GetStatusIdAsync(string statusName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for status with name {@Name}", statusName);
        var status = await _context.EmailStatuses
            .AsNoTracking()
            .FirstAsync(x => x.Name.Equals(statusName), cancellationToken);

        return status.Id;
    }

    private async Task<Email> CreateEmailAsync(CreateEmailDto dto, EmailTemplate template, EmailPriority emailPrirority)
    {
        _logger.LogInformation("Mapping template with id {@Id} to email", template.Id);
        var email = _mapper.Map<Email>(template);
        email.PrirorityId = emailPrirority.Id;
        email.Receiver = dto.Receiver;
        email.StatusId = (await _context.EmailStatuses
            .AsNoTracking()
            .FirstAsync(x => x.Name.Equals(EmailStatuses.New))).Id;

        _logger.LogInformation("Mapping parameters {@Parameters}", dto.Parameters);
        foreach (var (key, value) in dto.Parameters)
        {
            email.Subject = email.Subject.Replace(key, value);
            email.Body = email.Body.Replace(key, value);
        }

        return email;
    }

    public async Task<IEnumerable<Email>> GetAllWithStatusAsync(string statusName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for status with name {@Name}", statusName);
        var statusId = (await _context.EmailStatuses
            .FirstAsync(x => x.Name.Equals(statusName), cancellationToken)).Id;

        _logger.LogInformation("Fetching emails for status with id {@Id}", statusId);
        var emails = await _context.Emails
            .AsNoTracking()
            .Where(x => x.StatusId.Equals(statusId))
            .ToListAsync(cancellationToken);

        return emails;
    }

    public async Task<IEnumerable<EmailPriority>> GetAllPrioritiesAsync(CancellationToken cancellationToken = default)
    {
        var priotiries = await _context.EmailPriorities.ToListAsync(cancellationToken);

        return priotiries;
    }

    public async Task<bool> UpdateEmailsAsync(IEnumerable<Email> emails, CancellationToken cancellationToken = default)
    {
        _context.Emails.UpdateRange(emails);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
