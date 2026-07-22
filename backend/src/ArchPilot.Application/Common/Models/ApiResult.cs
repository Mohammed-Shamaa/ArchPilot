namespace ArchPilot.Application.Common.Models;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResult<T> SuccessResult(T data, string? message = null)
    {
        return new ApiResult<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static ApiResult<T> Failure(string message, List<string>? errors = null)
    {
        return new ApiResult<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
