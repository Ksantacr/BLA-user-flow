namespace BLA.UserFlow.Application.OperationResult;

public class Result<T>
{
    public bool Succeeded { get; protected set; }

    public T? Value { get; set; }
    public string[] Errors { get; protected set; }

    public static Result<T> Success(T value)
    {
        return new Result<T> { Succeeded = true, Value = value };
    }

    public static Result<T> Failure(params string[] errors)
    {
        return new Result<T> { Succeeded = false, Errors = errors };
    }
}