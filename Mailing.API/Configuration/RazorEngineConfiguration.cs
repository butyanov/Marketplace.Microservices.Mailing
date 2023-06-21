using Microsoft.AspNetCore.Mvc.Razor;

namespace Mailing.API.Configuration;

public static class RazorEngineConfiguration
{
    public static IServiceCollection AddRazorEngineConfiguration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMvc()
            .AddRazorRuntimeCompilation();
        
        serviceCollection.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Clear();
            options.ViewLocationFormats.Add("/LetterTemplates/{0}.cshtml");
        });

        return serviceCollection;
    }
}