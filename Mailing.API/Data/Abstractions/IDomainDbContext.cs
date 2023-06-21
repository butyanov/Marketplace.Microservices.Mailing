using Mailing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mailing.API.Data.Abstractions;

public interface IDomainDbContext
{
    public DbSet<Letter> Letters { get; set; }

    public Task<bool> SaveEntitiesAsync();
}