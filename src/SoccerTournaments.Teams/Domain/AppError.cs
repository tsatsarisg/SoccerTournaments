using FluentResults;

namespace SoccerTournaments.Teams;

public enum ErrorType
{
    Validation,
    NotFound,
    Unknown
}

public class AppError : Error
{
    public ErrorType Type { get; }

    public AppError(string message, ErrorType type = ErrorType.Unknown) : base(message)
    {
        Type = type;
    }

    public static AppError Validation(string message) => new(message, ErrorType.Validation);
    public static AppError NotFound(string message) => new(message, ErrorType.NotFound);
    public static AppError Unknown(string message) => new(message, ErrorType.Unknown);
}
