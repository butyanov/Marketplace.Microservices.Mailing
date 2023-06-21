using AutoMapper;
using Mailing.API.Authorization;
using Mailing.API.Dto;
using Mailing.API.Models;
using Mailing.API.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Mailing.API.Controllers;

[ApiController]
[Route("api/mail")]
public class MailController : ControllerBase
{
    private readonly IMailSenderService _mailSenderService;
    private readonly IMapper _mapper;
    
    public MailController(IMailSenderService mailSenderService, IMapper mapper)
    {
        _mailSenderService = mailSenderService;
        _mapper = mapper;
    }
    
    [Permissions(UserPermissionsPresets.Moderator)]
    [HttpPost("send")]
    public async Task Send([FromBody]SendMailDto dto)
    {
        var letter = _mapper.Map<Letter>(dto);
        await _mailSenderService.SendAsync(letter);
    }
}