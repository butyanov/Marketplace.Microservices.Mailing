using Mailing.API.Data.Abstractions;
using Mailing.API.Data.Configuration.Extensions;
using Mailing.API.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mailing.API.Data;

public class MailingDbContext : DbContext, IDomainDbContext
{
    private readonly JsonOptions _jsonOptions;
    public DbSet<Letter> Letters { get; set; }
    
    public MailingDbContext(
        DbContextOptions<MailingDbContext> options, IOptions<JsonOptions> jsonOptions) : base(options)
    {
        _jsonOptions = jsonOptions.Value;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var letterEntityBuilder = builder.Entity<Letter>();
        letterEntityBuilder.HasKey(l => l.LetterId);
        letterEntityBuilder.Property(l => l.LetterId).HasDefaultValueSql("gen_random_uuid()");
        letterEntityBuilder.Property(l => l.Properties).HasJsonConversion(_jsonOptions.SerializerOptions);
        letterEntityBuilder.Property(l => l.ArchivedAt).HasDefaultValueSql("NOW()");

    }
    
    public async Task<bool> SaveEntitiesAsync()
    {
        await base.SaveChangesAsync();
        return true;
    }
}