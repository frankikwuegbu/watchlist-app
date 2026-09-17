namespace Application.Common;

public class Result
{
    public bool Status { get; set; }
    public string? Message { get; set; }
    public object? Entity { get; set; }
    public object? SecondaryEntity { get; set; } = null;

    public static Result Success(string message, object? entity = null)
    {
        return new Result
        {
            Status = true,
            Message = message,
            Entity = entity
        };
    }

    public static Result Success(string message, object entity1, object entity2)
    {
        return new Result
        {
            Status = true,
            Message = message,
            Entity = entity1,
            SecondaryEntity = entity2
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

    public static Result<T> Success<T>(string message, T entity) => Result<T>.Success(message, entity);
    public static Result<T> Failure<T>(string message, T? entity = default) => Result<T>.Failure(message, entity);
    public static Result<T1, T2> Success<T1, T2>(string message, T1 entity1, T2 entity2)
        => Result<T1, T2>.Success(message, entity1, entity2);
    public static Result<T1, T2> Failure<T1, T2>(string message, T1? entity1 = default, T2? entity2 = default)
        => Result<T1, T2>.Failure(message, entity1, entity2);
}

public class Result<T> : Result
{
    public T? Type { get; set; }

    public static Result<T> Success(string message, T entity)
    {
        return new Result<T>
        {
            Status = true,
            Message = message,
            Entity = entity
        };
    }

    public static Result<T> Failure(string message, T? entity = default)
    {
        return new Result<T>
        {
            Status = false,
            Message = message,
            Entity = entity
        };
    }
}

public class Result<T1, T2> : Result
{
    public T1? Entity1 { get; set; }
    public T2? Entity2 { get; set; }

    public static Result<T1, T2> Success(string message, T1 entity1, T2 entity2)
    {
        return new Result<T1, T2>
        {
            Status = true,
            Message = message,
            Entity = entity1,
            Entity2 = entity2
        };
    }

    public static Result<T1, T2> Failure(string message, T1? entity1 = default, T2? entity2 = default)
    {
        return new Result<T1, T2>
        {
            Status = false,
            Message = message,
            Entity = entity1,
            SecondaryEntity = entity2
        };
    }
}