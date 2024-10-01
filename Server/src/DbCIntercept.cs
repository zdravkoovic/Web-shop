using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebShop.src;

public class DbCIntercept(ILogger<DbCIntercept> _logger) : DbCommandInterceptor
{
    private readonly ILogger<DbCIntercept> logger = _logger;

    public override async ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation("Executing Command: {CommandText}", command.CommandText);
        return await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }
}