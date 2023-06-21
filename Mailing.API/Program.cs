using Hangfire;
using Mailing.API.Configuration;
using Mailing.API.Data;
using Mailing.API.Data.Abstractions;
using Mailing.API.Middleware;
using Mailing.API.Services;
using Mailing.API.Services.Abstractions;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDbContext<IDomainDbContext, MailingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddControllers();
services
    .AddHttpContextAccessor()
    .AddRazorEngineConfiguration()
    .AddAutoMapper(typeof(Program).Assembly)
    .AddCustomValidation()
    .AddSingleton<ITempDataProvider, CookieTempDataProvider>()
    .AddScoped<IRendererService, RazorPageRendererService>()
    .AddLetterBuilders()
    .AddScoped<IMailSenderService, TemplateMailSenderService>();

services.AddCustomSwaggerConfiguration(builder.Configuration);

services.AddHangfireConfiguration(builder.Configuration);

services.AddCustomAuthentication(builder.Configuration);
services.AddAuthorization();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
var app = builder.Build();

await app.TryMigrateDatabase();

app.UseMiddleware<ExceptionHandlingMiddleware>();
/*app.UseMiddleware<DbTransactionsMiddleware>();*/

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard();

ConfigureHangfire.AddHangfireSendEmailJobs();

app.Run();