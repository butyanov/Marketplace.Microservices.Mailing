using Hangfire;
using Hangfire.PostgreSql;
using Mailing.API.HangfireJobs;

namespace Mailing.API.Configuration;

public static class ConfigureHangfire
{
    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
            config
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(configuration.GetConnectionString("HangfireConnection")));

        services.AddHangfireServer(opt =>
        {
            opt.Queues = new[] {"send-email", "default"};
            opt.WorkerCount = 1;
        });
        
        services.AddTransient<SendEmailsJob>();
        
        return services;
    }
    public static void AddHangfireSendEmailJobs()
    {
        RecurringJob.AddOrUpdate<SendEmailsJob>(SendEmailsJob.Id, service =>
            service.SendEmails(), Cron.Hourly);
    }
}