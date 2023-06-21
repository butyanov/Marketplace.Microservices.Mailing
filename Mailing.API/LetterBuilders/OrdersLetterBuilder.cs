using Mailing.API.Enums;
using Mailing.API.Services.Abstractions;

namespace Mailing.API.LetterBuilders;

public class OrdersLetterBuilder : InfoLetterBuilder
{
    public override TemplateCode TemplateCode => TemplateCode.Order;
    
    public OrdersLetterBuilder(IRendererService razorPageStringifyService) : base(razorPageStringifyService)
    {
    }
}