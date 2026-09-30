namespace Sinestify.WebAPI.Services;

public enum ApiOperationStatus
{
    Success,
    Invalid,
    NotFound
}

public sealed record ApiOperationResult<T>(ApiOperationStatus Status, T? Value, IReadOnlyList<string> Errors);