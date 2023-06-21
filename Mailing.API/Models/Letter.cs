using Mailing.API.Enums;

namespace Mailing.API.Models;

public class Letter
{
    public Guid LetterId { get; set; }
    public LetterStatus Status { get; set; }
    public string Recipient { get; set; }
    public TemplateCode TemplateCode { get; set; }
    public Dictionary<string, object> Properties { get; set; }
    public DateTime ArchivedAt { get; set; }
}