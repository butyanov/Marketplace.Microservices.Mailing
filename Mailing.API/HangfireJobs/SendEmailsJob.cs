using Hangfire;
using Mailing.API.Data.Abstractions;
using Mailing.API.Enums;
using Mailing.API.Services.Abstractions;

namespace Mailing.API.HangfireJobs;

public class SendEmailsJob
{
    public const string Id = "SendEmailsJob";
    private readonly IDomainDbContext _dbContext;
    private readonly IMailSenderService _mailSenderService;
    
    
    public SendEmailsJob(IMailSenderService mailSenderService, IDomainDbContext dbContext)
    {
        _mailSenderService = mailSenderService;
        _dbContext = dbContext;
    }
    
    [Queue("send-email")]
    public async Task SendEmails()
    {
        var unsentLetters = _dbContext.Letters.Where(l => l.Status == LetterStatus.Unsent).Take(20);

        if (!unsentLetters.Any())
            return;
        
        foreach (var unsentLetter in unsentLetters)
            await _mailSenderService.SendAsync(unsentLetter);

        _dbContext.Letters.RemoveRange(unsentLetters);
        await _dbContext.SaveEntitiesAsync();
    }
}