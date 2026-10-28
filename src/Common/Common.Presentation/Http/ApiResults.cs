using Common.Domain;
using Microsoft.AspNetCore.Http;

namespace Common.Presentation.Http;

public static class ApiResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("A successful result can't be converted to a problem.");
        }

        var error = result.Error;

        return Results.Problem(
            title: error.Code,
            detail: error.Description,
            type: GetType(error.Type),
            statusCode: GetStatusCode(error.Type),
            extensions: GetErrors(error));
    }

    private static string GetType(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        ErrorType.NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
        ErrorType.Conflict => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
        _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1",
    };

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError,
    };

    private static Dictionary<string, object?>? GetErrors(Error error) =>
        error is ValidationError validationError
            ? new Dictionary<string, object?> { ["errors"] = validationError.Errors }
            : null;
}
