using Mailing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Mailing.API.Middleware;

public class DbTransactionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DbTransactionsMiddleware> _logger;

    public DbTransactionsMiddleware(ILogger<DbTransactionsMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    
    public async Task Invoke(HttpContext context, MailingDbContext dbContext)
    {
        var execStrategy = dbContext.Database.CreateExecutionStrategy();
        await execStrategy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();
        
                try
                {
                    _logger.LogInformation($"Begin transaction {transaction.TransactionId}");

                     await _next(context);

                    await transaction.CommitAsync();

                    _logger.LogInformation($"Committed transaction {transaction.TransactionId}");
                    
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Rollback transaction executed {transaction.TransactionId}");

                    await transaction.RollbackAsync();

                    _logger.LogError(e.Message, e.StackTrace);

                    throw;
                }
            });
    }
}