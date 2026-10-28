using System.Diagnostics;
using Common.Application.Messaging;
using Common.Domain;
using Microsoft.Extensions.Logging;

namespace Common.Application.Behaviors;

internal static partial class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken) =>
            ExecuteAsync(logger, typeof(TCommand).Name, () => innerHandler.Handle(command, cancellationToken));
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public Task<Result> Handle(TCommand command, CancellationToken cancellationToken) =>
            ExecuteAsync(logger, typeof(TCommand).Name, () => innerHandler.Handle(command, cancellationToken));
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken) =>
            ExecuteAsync(logger, typeof(TQuery).Name, () => innerHandler.Handle(query, cancellationToken));
    }

    private static async Task<TResult> ExecuteAsync<TResult>(ILogger logger, string requestName, Func<Task<TResult>> next)
        where TResult : Result
    {
        LogProcessing(logger, requestName);
        var startedAt = Stopwatch.GetTimestamp();

        var result = await next();

        var elapsedMs = Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds;
        if (result.IsSuccess)
        {
            LogCompleted(logger, requestName, elapsedMs);
        }
        else
        {
            LogFailed(logger, requestName, elapsedMs, result.Error.Code, result.Error.Description);
        }

        return result;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Processing {RequestName}")]
    private static partial void LogProcessing(ILogger logger, string requestName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Completed {RequestName} in {ElapsedMs:0.0} ms")]
    private static partial void LogCompleted(ILogger logger, string requestName, double elapsedMs);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed {RequestName} in {ElapsedMs:0.0} ms with {ErrorCode}: {ErrorDescription}")]
    private static partial void LogFailed(ILogger logger, string requestName, double elapsedMs, string errorCode, string errorDescription);
}
