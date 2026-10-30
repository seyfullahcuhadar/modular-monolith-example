using Common.Application.Messaging;
using Common.Domain;
using FluentValidation;

namespace Common.Application.Behaviors;

internal static class ValidationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var validationError = await ValidateAsync(command, validators, cancellationToken);

            return validationError is null
                ? await innerHandler.Handle(command, cancellationToken)
                : Result.Failure<TResponse>(validationError);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var validationError = await ValidateAsync(command, validators, cancellationToken);

            return validationError is null
                ? await innerHandler.Handle(command, cancellationToken)
                : Result.Failure(validationError);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        IEnumerable<IValidator<TQuery>> validators)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            var validationError = await ValidateAsync(query, validators, cancellationToken);

            return validationError is null
                ? await innerHandler.Handle(query, cancellationToken)
                : Result.Failure<TResponse>(validationError);
        }
    }

    private static async Task<ValidationError?> ValidateAsync<TRequest>(
        TRequest request,
        IEnumerable<IValidator<TRequest>> validators,
        CancellationToken cancellationToken)
    {
        var validatorArray = validators.ToArray();
        if (validatorArray.Length == 0)
        {
            return null;
        }

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(validatorArray.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errors = results
            .SelectMany(r => r.Errors)
            .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
            .ToList();

        return errors.Count == 0 ? null : new ValidationError(errors);
    }
}
