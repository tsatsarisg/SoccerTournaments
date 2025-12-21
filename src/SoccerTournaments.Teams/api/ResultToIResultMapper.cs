using FluentResults;
using Microsoft.AspNetCore.Http;

namespace SoccerTournaments.Teams.Api;

public record ErrorResponse(ErrorType Type, string Message);

public static class ResultToIResultMapper
{
    public static IResult ToIResult<T>(this Result<T> result, Func<T, IResult>? onSuccess = null)
    {
        if (result.IsSuccess)
        {
            return onSuccess != null ? onSuccess(result.Value!) : Results.Ok(result.Value!);
        }

        var errorMessage = string.Join(", ", result.Errors.Select(e => e.Message));
        var errorType = DetermineErrorType(result);
        var statusCode = GetStatusCode(errorType);

        var errorResponse = new ErrorResponse(errorType, errorMessage);
        
        return Results.Json(errorResponse, statusCode: statusCode);
    }

    private static ErrorType DetermineErrorType(ResultBase result)
    {
        var appError = result.Errors.OfType<AppError>().FirstOrDefault();
        
        return appError?.Type switch
        {
            ErrorType.Validation => ErrorType.Validation,
            ErrorType.NotFound => ErrorType.NotFound,
            _ => ErrorType.Unknown
        };
    }

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unknown => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };
}
