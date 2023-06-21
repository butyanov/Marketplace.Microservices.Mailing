using Mailing.API.Models;

namespace Mailing.API.Services.Abstractions;

public interface IMailSenderService
{
    public Task SendAsync(Letter letter);
}