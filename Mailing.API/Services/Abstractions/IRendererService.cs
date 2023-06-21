namespace Mailing.API.Services.Abstractions;

public interface IRendererService
{
    public Task<string> Render<TModel>(string templateName, TModel model);
}