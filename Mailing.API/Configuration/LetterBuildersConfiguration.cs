using Mailing.API.LetterBuilders;
using Mailing.API.LetterBuilders.Abstractions;

namespace Mailing.API.Configuration;

public static class LetterBuildersConfiguration
{
    public static IServiceCollection AddLetterBuilders(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddScoped<AbstractLetterBuilder, InfoLetterBuilder>()
            .AddScoped<AbstractLetterBuilder, OrdersLetterBuilder>();
}