using System.Collections.Immutable;
using System.Text;

namespace RGamaFelix.ServiceResponse;

/// <summary>
///     Represent the serviceResponse of a service execution that does return a value
/// </summary>
public class ServiceResultOf<T> : IServiceResultOf<T>
{
    private List<string> _errors;

    private ServiceResultOf(T? serviceResponse, IEnumerable<string>? errors, Exception? exception,
        ResultTypeCode errorType)
    {
        Data = serviceResponse;
        _errors = new List<string>(errors ?? new List<string>());
        ResultType = errorType;
        Exception = exception;
    }

    /// <summary>
    ///     The serviceResponse of a successful service execution
    /// </summary>
    public T? Data { get; }

    /// <summary>
    ///     Get the the list of errors occurred in the service execution
    /// </summary>
    public IReadOnlyCollection<string> Errors => _errors.ToImmutableArray();

    public Exception? Exception { get; }

    /// <summary>
    ///     Get a flag indicating if the service call has succeeded
    /// </summary>
    public bool IsSuccess => ResultType.IsSuccessCode;

    /// <summary>
    ///     General error type
    /// </summary>
    public ResultTypeCode ResultType { get; }

    /// <summary>
    ///     Return a failed service serviceResponse
    /// </summary>
    /// <param name="errors">List of error messages occurred during the service execution</param>
    /// <param name="errorType">General error type</param>
    /// <returns></returns>
    public static IServiceResultOf<T> Fail(IEnumerable<string> errors, ResultTypeCode errorType)
    {
        if (errorType.IsSuccessCode)
        {
            throw new ArgumentException("Success code cannot be used for error result");
        }

        return new ServiceResultOf<T>(default, errors, null, errorType);
    }

    /// <summary>
    ///     Return a failed service serviceResponse
    /// </summary>
    /// <param name="error">Error message occurred during the service execution</param>
    /// <param name="errorType">General error type</param>
    /// <remarks>Accessory method for when only one error message is generated</remarks>
    /// <returns></returns>
    public static IServiceResultOf<T> Fail(string error, ResultTypeCode errorType)
    {
        if (errorType.IsSuccessCode)
        {
            throw new ArgumentException("Success code cannot be used for error result");
        }

        return new ServiceResultOf<T>(default, new[] { error }, null, errorType);
    }

    public static IServiceResultOf<T> Fail(Exception exception)
    {
        return new ServiceResultOf<T>(default, new[] { exception.Message }, exception, ResultTypeCode.UnexpectedError);
    }

    /// <summary>
    ///     Return a success service serviceResponse
    /// </summary>
    /// <param name="serviceResponse">The serviceResponse of the service execution</param>
    /// <param name="resultTypeCode">General result of the service execution</param>
    /// <returns></returns>
    public static IServiceResultOf<T> Success(T serviceResponse, ResultTypeCode resultTypeCode)
    {
        if (!resultTypeCode.IsSuccessCode)
        {
            throw new ArgumentException("Error code cannot be used for success result");
        }

        return new ServiceResultOf<T>(serviceResponse, null, null, resultTypeCode);
    }

    public override string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.AppendLine($"IsSuccess: {IsSuccess}({ResultType.Name})");
        if (!IsSuccess)
        {
            strBuilder.AppendLine($"Errors: {ToErrorString()}");
            if (Exception is not null)
            {
                strBuilder.AppendLine($"Exception: {Exception.ToString()}");
            }
        }
        else
        {
            strBuilder.AppendLine($"Data: {Data!.ToString()}");
        }

        return strBuilder.ToString();
    }

    public static implicit operator Exception?(ServiceResultOf<T> resultOf)
    {
        return resultOf.Exception;
    }

    public static implicit operator T?(ServiceResultOf<T> resultOf)
    {
        return resultOf.Data;
    }

    /// <summary>
    ///     Returns a string describing the error.
    /// </summary>
    /// <returns></returns>
    public string ToErrorString()
    {
        var strBuilder = new StringBuilder();
        foreach (var error in _errors)
        {
            strBuilder.AppendLine($"{error};");
        }

        return strBuilder.ToString();
    }
}
