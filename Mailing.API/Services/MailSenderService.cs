using Mailing.API.Data.Abstractions;
using Mailing.API.Enums;
using Mailing.API.Exceptions;
using Mailing.API.LetterBuilders.Abstractions;
using Mailing.API.Models;
using Mailing.API.Services.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Mailing.API.Services;

public class TemplateMailSenderService : IMailSenderService
{
    private readonly string _senderName;
    private readonly string _host;
    private readonly string _password;
    private readonly int _port;
    private readonly string _username;
    private readonly IEnumerable<AbstractLetterBuilder> _letterBuilders;
    private readonly IDomainDbContext _dbContext;

    public TemplateMailSenderService(IConfiguration configuration, IEnumerable<AbstractLetterBuilder> letterBuilders, IDomainDbContext dbContext)
    {
        _username = configuration.GetValue<string>("MailCredentials:Username")!;
        _password = configuration.GetValue<string>("MailCredentials:Password")!;
        _host = configuration.GetValue<string>("Mailing:Host")!;
        _port = configuration.GetValue<int>("Mailing:Port");
        _senderName = configuration.GetValue<string>("Mailing:Sender")!;
        _letterBuilders = letterBuilders;
        _dbContext = dbContext;
    }

    public async Task SendAsync(Letter letter)
    {
        var builder = TryFindBuilderByTemplateCode(letter.TemplateCode);

        if (builder == null)
            throw new BadRequestException("INVALID_TEMPLATE_CODE");
        
        builder.ProcessProperties(letter.Properties);
        var letterBody = await builder.Build(letter.Properties);
    
        await SendAsyncInternal(letterBody, letter);
    }

    private async Task SendAsyncInternal(string html, Letter letter)
    {
        using var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_senderName, _username));
        emailMessage.To.Add(new MailboxAddress(string.Empty, letter.Recipient));
        emailMessage.Subject = letter.Properties["Subject"].ToString();

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = html
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        
        using var client = new SmtpClient();
        {
            try
            {
                await client.ConnectAsync(_host, _port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception)
            {
                await ArchiveLetter(letter);
            }
        }
    }

    private async Task ArchiveLetter(Letter letter)
    {
        if (_dbContext.Letters.Any(l => l.LetterId == letter.LetterId))
            return;
        
        letter.Status = LetterStatus.Unsent;
        _dbContext.Letters.Add(letter);
        
        await _dbContext.SaveEntitiesAsync();
    }
    
    private AbstractLetterBuilder? TryFindBuilderByTemplateCode(TemplateCode templateCode)
    {
        return _letterBuilders.FirstOrDefault(builder =>
        {
            var propertyInfo = builder.GetType().GetProperty("TemplateCode");
            var value = (TemplateCode)propertyInfo!.GetValue(builder)!;
            return value == templateCode;
        });
    }
}