using System.Text;

namespace RGamaFelix.ServiceResponse;

/// <summary>
///     Represent the response of a service execution that does not return a value
/// </summary>
public sealed class ServiceResult : ServiceResultBase
{
    private ServiceResult(IEnumerable<string>? errors, Exception? exception, ResultTypeCode errorType) : base(errors,
        exception, errorType)
    {
    }

    /// <summary>
    ///     Return a failed service serviceResponse
    /// </summary>
    /// <param name="errors">List of error messages occurred during the service execution</param>
    /// <param name="errorType">General error type</param>
    /// <returns></returns>
    public static IServiceResult Fail(IEnumerable<string> errors, ResultTypeCode resultType)
    {
        if (resultType.IsSuccessCode)
        {
            throw new ArgumentException("Success code cannot be used for error result");
        }

        return new ServiceResult(errors, null, resultType);
    }

    /// <summary>
    ///     Return a failed service serviceResponse
    /// </summary>
    /// <param name="error">Error message occurred during the service execution</param>
    /// <param name="resultType">General error type</param>
    /// <remarks>Accessory method for when only one error message is generated</remarks>
    /// <returns></returns>
    public static IServiceResult Fail(string error, ResultTypeCode resultType)
    {
        if (resultType.IsSuccessCode)
        {
            throw new ArgumentException("Success code cannot be used for error result");
        }

        return new ServiceResult(new[] { error }, null, resultType);
    }

    public static ServiceResult Fail(Exception exception)
    {
        return new ServiceResult(new[] { exception.Message }, exception, ResultTypeCode.UnexpectedError);
    }

    /// <summary>
    ///     Return a success service serviceResponse
    /// </summary>
    /// <returns></returns>
    public static ServiceResult Success()
    {
        return new ServiceResult(null, null, ResultTypeCode.Ok);
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

        return strBuilder.ToString();
    }

    public static implicit operator Exception?(ServiceResult result)
    {
        return result.Exception;
    }
}
