using FluentValidation;
using FluentValidation.AspNetCore;

namespace Mailing.API.Configuration;

public static class ValidationConfiguration
{
    public static IServiceCollection AddCustomValidation(this IServiceCollection serviceCollection) =>
        serviceCollection.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(Program).Assembly);
}