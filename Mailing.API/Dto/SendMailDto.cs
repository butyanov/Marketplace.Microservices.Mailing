using FluentValidation;
using Mailing.API.Enums;

namespace Mailing.API.Dto;

public class SendMailDtoValidator : AbstractValidator<SendMailDto>
{
    public SendMailDtoValidator()
    {
        RuleFor(m => m.TemplateCode)
            .IsInEnum()
            .WithMessage("INCORRECT_TEMPLATE_CODE");
        RuleFor(m => m.Recipient)
            .NotEmpty()
            .WithMessage("EMPTY_FIELD")
            .EmailAddress()
            .WithMessage("INVALID_EMAIL");
        RuleFor(m => m.Properties)
            .NotEmpty()
            .WithMessage("EMPTY_FIELD")
            .Must(p => p.Any())
            .WithMessage("NO_PROPS_PROVIDED");

    }
}

public record SendMailDto(string Recipient, TemplateCode TemplateCode, Dictionary<string, object> Properties);
