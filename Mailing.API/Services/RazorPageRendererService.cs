using Mailing.API.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Mailing.API.Services;

public class RazorPageRendererService : IRendererService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly HttpContext? _httpContext;
    

    public RazorPageRendererService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _razorViewEngine = serviceProvider.GetService<IRazorViewEngine>()!;
        _tempDataProvider = serviceProvider.GetService<ITempDataProvider>()!;
        _httpContext = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
    }

    public async Task<string> Render<TModel>(string templateName, TModel model)
    {
        var actionContext = new ActionContext(_httpContext ?? new DefaultHttpContext {RequestServices = _serviceProvider}, new RouteData(), new ActionDescriptor());
        
        await using var sw = new StringWriter();
        
        var viewResult = _razorViewEngine.FindView(actionContext, templateName, false);

        if (viewResult.View == null)
            throw new ArgumentNullException($"{templateName.ToUpper()}_DOES_NOT_MATCH_ANY_TEMPLATE");

        var viewDictionary = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewDictionary,
            new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
            sw,
            new HtmlHelperOptions()
        );
        
        await viewResult.View.RenderAsync(viewContext);
        
        return sw.ToString();
    }
}