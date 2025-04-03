using System.Net;

namespace FluxConfig.Storage.Api.Extensions;

public static partial class LoggerExtensions
{
    #region Info

    [LoggerMessage(
        LogLevel.Information,
        Message = "[{CallId}] [{CurTime}] Start executing call. Method: {MethodName}.")]
    public static partial void LogCallStart(this ILogger logger,
        string callId,
        DateTime curTime,
        string methodName);


    [LoggerMessage(
        LogLevel.Information,
        Message = "[{CallId}] [{CurTime}] Successfully ended executing call. Method: {MethodName}.")]
    public static partial void LogSuccessCallEnd(this ILogger logger,
        string callId,
        DateTime curTime,
        string methodName);


    [LoggerMessage(
        LogLevel.Information,
        Message = "[{CallId}] [{CurTime}] Passed headers:\n{HeadersMeta}")]
    public static partial void LogCallHeadersMeta(this ILogger logger,
        string callId,
        DateTime curTime,
        string? headersMeta);

    #endregion

    #region Error

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "[{CallId}] [{CurTime}] Validation error: \n{Violations}"
    )]
    public static partial void LogDomainBadRequestError(this ILogger logger,
        string callId,
        DateTime curTime,
        string violations);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "[{CallId}] [{CurTime}] Service configuration not found for Key: {Key} | Tag: {Tag}"
    )]
    public static partial void LogDomainNotFoundError(this ILogger logger,
        string callId,
        DateTime curTime,
        string? key, string? tag);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "[{CallId}] [{CurTime}] Duplicate configuration creation for Key: {Key} | Tag: {Tag}"
    )]
    public static partial void
        LogDomainDuplicateConfigError(this ILogger logger,
            string callId,
            DateTime curTime,
            string? key,
            string? tag);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "[{CallId}] [{CurTime}] Unexpected exception occured during rpc call."
    )]
    public static partial void LogInternalError(this ILogger logger,
        Exception exception,
        string callId,
        DateTime curTime);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "[{CallId}] [{CurTime}] Unable to get the response from FC Management service for address: {Address}"
    )]
    public static partial void LogFcManagementUnavailable(
        this ILogger logger,
        string callId,
        DateTime curTime,
        Uri? address);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message =
            "[{CallId}] [{CurTime}] Unexpected response from FC Management service for address: {Address} with status-code: {StatusCode} - {NamedStatusCode}"
    )]
    public static partial void LogFcManagementUnexpectedResponse(
        this ILogger logger,
        string callId,
        DateTime curTime,
        Uri? address,
        int statusCode,
        HttpStatusCode namedStatusCode);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message =
            "[{CallId}] [{CurTime}] Invalid internal api-key authentication metadata needed to access FC Management api. X-API-KEY: {ApiKey}"
    )]
    public static partial void LogInternalServiceOutgoingAuthError(this ILogger logger,
        string callId,
        DateTime curTime,
        string apiKey);
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        Message =
            "[{CallId}] [{CurTime}] Given invalid internal api-key authentication metadata needed to access FC Storage api. X-API-KEY: {ApiKey}"
    )]
    public static partial void LogInternalServiceIncomingAuthError(this ILogger logger,
        string callId,
        DateTime curTime,
        string apiKey);

    
    [LoggerMessage(
        Level = LogLevel.Error,
        Message =
            "[{CallId}] [{CurTime}] Invalid service api-key authentication metadata needed to access configuration. X-API-KEY: {ApiKey}."
    )]
    public static partial void LogClientServiceAuthError(this ILogger logger,
        string callId,
        DateTime curTime,
        string apiKey);

    #endregion
}