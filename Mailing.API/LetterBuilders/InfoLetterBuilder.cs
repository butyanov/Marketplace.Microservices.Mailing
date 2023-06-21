using Mailing.API.Enums;
using Mailing.API.LetterBuilders.Abstractions;
using Mailing.API.Services.Abstractions;

namespace Mailing.API.LetterBuilders;

public class InfoLetterBuilder : AbstractLetterBuilder
{
    public override TemplateCode TemplateCode => TemplateCode.Info;
    

    public InfoLetterBuilder(IRendererService razorPageStringifyService) : base(razorPageStringifyService)
    {
    }
    
    public override void ProcessProperties(Dictionary<string, object> properties)
    {
    }
}