using System.Text;

namespace RGamaFelix.ServiceResponse;

/// <summary>
///   Represents the result of a service operation that can include a result of type <typeparamref name="T" />, a
///   collection of errors, an exception, and a result status represented by <see cref="ResultTypeCode" />.
/// </summary>
/// <typeparam name="T">The type of the data returned from a service operation.</typeparam>
public class ServiceResultOf<T> : IServiceResultOf<T>
{
  private readonly List<string> _errors;

  private ServiceResultOf(T? data, IEnumerable<string>? errors, Exception? exception, ResultTypeCode resultType)
  {
    Data = data;
    _errors = errors?.ToList() ?? new List<string>();
    ResultType = resultType;
    Exception = exception;
  }

  /// <summary>The data of a successful service execution</summary>
  public T? Data { get; }

  /// <summary>Get the list of errors occurred in the service execution</summary>
  public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

  /// <summary>Represents the exception that occurred during the service execution.</summary>
  public Exception? Exception { get; }

  /// <summary>Get a flag indicating if the service call has succeeded</summary>
  public bool IsSuccess => ResultType.IsSuccessCode;

  /// <summary>General result type</summary>
  public ResultTypeCode ResultType { get; }

  /// <summary>
  ///   Provides a string representation of the service result, including success status, data, errors, and exception
  ///   details if present.
  /// </summary>
  /// <returns>
  ///   A formatted string containing service result information such as success status, result type, errors, data,
  ///   and exception details.
  /// </returns>
  public override string ToString()
  {
    var strBuilder = new StringBuilder();
    strBuilder.AppendLine($"IsSuccess: {IsSuccess}({ResultType.Name})");

    if (!IsSuccess)
    {
      strBuilder.AppendLine($"Errors: {ToErrorString()}");

      if (Exception is not null)
      {
        strBuilder.AppendLine($"Exception: {Exception}");
      }
    }
    else
    {
      strBuilder.AppendLine($"Data: {Data?.ToString() ?? "null"}");
    }

    return strBuilder.ToString();
  }

  /// <summary>Return a failed service result</summary>
  /// <param name="errors">List of error messages occurred during the service execution</param>
  /// <param name="resultType">General result type</param>
  public static IServiceResultOf<T> Fail(IEnumerable<string> errors, ResultTypeCode resultType)
  {
    ArgumentNullException.ThrowIfNull(errors);
    EnsureErrorCode(resultType);

    return new ServiceResultOf<T>(default, errors, null, resultType);
  }

  /// <summary>Return a failed service result</summary>
  /// <param name="error">Error message occurred during the service execution</param>
  /// <param name="resultType">General result type</param>
  /// <remarks>Accessory method for when only one error message is generated</remarks>
  public static IServiceResultOf<T> Fail(string error, ResultTypeCode resultType)
  {
    ArgumentException.ThrowIfNullOrEmpty(error);
    EnsureErrorCode(resultType);

    return Fail(new[] { error }, resultType);
  }

  /// <summary>Creates a failed service result using the provided exception details as the error information.</summary>
  /// <param name="exception">The exception to be included in the service result, providing error details and context.</param>
  /// <returns>
  ///   An instance of <see cref="IServiceResultOf{T}" /> representing the failure, containing the exception details
  ///   and an "UnexpectedError" result type.
  /// </returns>
  public static IServiceResultOf<T> Fail(Exception exception)
  {
    ArgumentNullException.ThrowIfNull(exception);

    return new ServiceResultOf<T>(default, new[] { exception.Message }, exception, ResultTypeCode.UnexpectedError);
  }

  /// <summary>
  ///   Implicitly converts a ServiceResultOf instance to an Exception if an exception is present in the service
  ///   result.
  /// </summary>
  /// <param name="resultOf">The service result instance to be converted.</param>
  /// <returns>The Exception associated with the service result if present; otherwise, null.</returns>
  public static implicit operator Exception?(ServiceResultOf<T> resultOf)
  {
    return resultOf.Exception;
  }

  /// <summary>Implicitly converts a ServiceResultOf instance to the data of type T if data is present in the service result.</summary>
  /// <param name="resultOf">The service result instance to be converted.</param>
  /// <returns>The data of type T contained in the service result if present; otherwise, null.</returns>
  public static implicit operator T?(ServiceResultOf<T> resultOf)
  {
    return resultOf.Data;
  }

  /// <summary>Return a successful service result</summary>
  /// <param name="data">The data of the service execution</param>
  /// <param name="resultType">General result type of the service execution</param>
  public static IServiceResultOf<T> Success(T data, ResultTypeCode resultType)
  {
    EnsureSuccessCode(resultType);

    return new ServiceResultOf<T>(data, null, null, resultType);
  }

  /// <summary>
  ///   Converts the list of error messages into a single formatted string where each error is separated by a newline
  ///   and ends with a semicolon.
  /// </summary>
  /// <returns>
  ///   A formatted string containing the error messages, each followed by a semicolon and separated by newlines. If
  ///   no errors exist, an empty string is returned.
  /// </returns>
  public string ToErrorString()
  {
    if (_errors.Count == 0)
    {
      return string.Empty;
    }

    // Keep the trailing ';' per original behavior, each on a new line.
    return string.Join(Environment.NewLine, _errors.Select(e => $"{e};")) + Environment.NewLine;
  }

  // Centralized guards to remove duplication and make intent explicit.
  private static void EnsureErrorCode(ResultTypeCode resultType)
  {
    if (resultType.IsSuccessCode)
    {
      throw new ArgumentException("Success code cannot be used for error result");
    }
  }

  private static void EnsureSuccessCode(ResultTypeCode resultType)
  {
    if (!resultType.IsSuccessCode)
    {
      throw new ArgumentException("Error code cannot be used for success result");
    }
  }
}
