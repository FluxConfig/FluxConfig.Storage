using Microsoft.Extensions.Logging;

namespace FluxConfig.Storage.Domain.Extensions;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "[{CurTime}] Validation error: Invalid passed data Key=<{Key}> | Tag=<{Tag}>"
    )]
    public static partial void LogDomainBadRequestError(this ILogger logger, Exception exception, DateTime curTime,
        string? key, string? tag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "[{CurTime}] Service configuration not found: Key=<{Key}> | Tag=<{Tag}>"
    )]
    public static partial void LogDomainNotFoundError(this ILogger logger, Exception exception, DateTime curTime,
        string? key, string? tag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "[{CurTime}] Duplicate configuration creation: Key=<{Key}> | Tag=<{Tag}>"
    )]
    public static partial void
        LogDomainDuplicateConfigError(this ILogger logger, Exception exception, DateTime curTime, string? key,
            string? tag);
}