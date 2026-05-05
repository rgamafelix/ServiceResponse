using System.Text;

namespace RGamaFelix.ServiceResponse;

/// <summary>
///   Provides a concrete implementation of <see cref="IServiceResultOf{T}" /> to encapsulate the results of service operations.
/// </summary>
/// <typeparam name="T">The type of the data returned from the service operation.</typeparam>
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

  /// <inheritdoc />
  public T? Data { get; }

  /// <inheritdoc />
  public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

  /// <inheritdoc />
  public Exception? Exception { get; }

  /// <inheritdoc />
  public bool IsSuccess => ResultType.IsSuccessCode;

  /// <inheritdoc />
  public ResultTypeCode ResultType { get; }

  /// <summary>
  ///   Creates a failed service result with a collection of error messages.
  /// </summary>
  /// <param name="errors">The collection of error messages that occurred.</param>
  /// <param name="resultType">The semantic status code for the failure.</param>
  /// <returns>A new <see cref="IServiceResultOf{T}" /> instance representing the failure.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors" /> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when <paramref name="resultType" /> is a success code.</exception>
  public static IServiceResultOf<T> Fail(IEnumerable<string> errors, ResultTypeCode resultType)
  {
    ArgumentNullException.ThrowIfNull(errors);
    EnsureErrorCode(resultType);

    return new ServiceResultOf<T>(default, errors, null, resultType);
  }

  /// <summary>
  ///   Creates a failed service result with a single error message.
  /// </summary>
  /// <param name="error">The error message that occurred.</param>
  /// <param name="resultType">The semantic status code for the failure.</param>
  /// <returns>A new <see cref="IServiceResultOf{T}" /> instance representing the failure.</returns>
  /// <exception cref="ArgumentException">Thrown when <paramref name="error" /> is null or empty, or when <paramref name="resultType" /> is a success code.</exception>
  public static IServiceResultOf<T> Fail(string error, ResultTypeCode resultType)
  {
    ArgumentException.ThrowIfNullOrEmpty(error);
    EnsureErrorCode(resultType);

    return new ServiceResultOf<T>(default, [error], null, resultType);
  }

  /// <summary>
  ///   Creates a failed service result from an exception.
  /// </summary>
  /// <param name="exception">The exception that occurred during execution.</param>
  /// <returns>A new <see cref="IServiceResultOf{T}" /> instance representing the failure with <see cref="ResultTypeCode.UnexpectedError" />.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception" /> is null.</exception>
  public static IServiceResultOf<T> Fail(Exception exception)
  {
    ArgumentNullException.ThrowIfNull(exception);

    return new ServiceResultOf<T>(default, new[] { exception.Message }, exception, ResultTypeCode.UnexpectedError);
  }

  /// <summary>
  ///   Creates a successful service result.
  /// </summary>
  /// <param name="data">The result data of the operation.</param>
  /// <param name="resultType">The semantic status code for the success.</param>
  /// <returns>A new <see cref="IServiceResultOf{T}" /> instance representing the success.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="data" /> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when <paramref name="resultType" /> is an error code.</exception>
  public static IServiceResultOf<T> Success(T data, ResultTypeCode resultType)
  {
    ArgumentNullException.ThrowIfNull(data);
    EnsureSuccessCode(resultType);

    return new ServiceResultOf<T>(data, null, null, resultType);
  }

  /// <summary>
  ///   Returns a string that represents the current service result.
  /// </summary>
  /// <returns>A string representation of the success status, data, and errors.</returns>
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

  /// <summary>
  ///   Converts the collection of error messages into a single formatted string.
  /// </summary>
  /// <param name="separator">The string to use as a separator between error messages. Defaults to "; ".</param>
  /// <returns>A string containing the formatted error messages, or an empty string if no errors exist.</returns>
  public string ToErrorString(string separator = "; ")
  {
    if (_errors.Count == 0)
    {
      return string.Empty;
    }

    return string.Join(separator, _errors.Select(e => $"{e}"));
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
