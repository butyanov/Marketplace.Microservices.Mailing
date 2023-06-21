using Mailing.API.Enums;
using Mailing.API.Services.Abstractions;

namespace Mailing.API.LetterBuilders.Abstractions;

public abstract class AbstractLetterBuilder
{
    public abstract TemplateCode TemplateCode { get; }
    
    private readonly IRendererService _razorPageRendererService;
    
    protected AbstractLetterBuilder(IRendererService razorPageStringifyService)
    {
        _razorPageRendererService = razorPageStringifyService;
    }

    public virtual async Task<string> Build(Dictionary<string, object> properties) =>
        await _razorPageRendererService.Render(GetTemplateName(), properties);

    public abstract void ProcessProperties(Dictionary<string, object> properties);

    protected string GetTemplateName()
    {
        var propertyInfo = GetType().GetProperty("TemplateCode");
        var enumValue = (TemplateCode)propertyInfo!.GetValue(this)!;
        var templateCodeString = enumValue.ToString();

        return $"{templateCodeString}LetterTemplate";
    }

}