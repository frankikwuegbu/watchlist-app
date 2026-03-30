namespace Application.Common;

public class Result
{
    public bool Status { get; set; }
    public string? Message { get; set; }
    public object? Entity { get; set; }

    public static Result Success(string message, object? entity = null)
    {
        return new Result
        {
            Status = true,
            Message = message,
            Entity = entity
        };
    }

    public static Result Failure(string message, object? entity = null)
    {
        return new Result
        {
            Status = false,
            Message = message,
            Entity = entity
        };
    }
}
