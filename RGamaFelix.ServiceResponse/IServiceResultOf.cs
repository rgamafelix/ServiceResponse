namespace RGamaFelix.ServiceResponse;

/// <summary>
///   Defines the contract for a service result that encapsulates the outcome of an operation,
///   including the data, any errors that occurred, and the result status.
/// </summary>
/// <typeparam name="T">The type of the data returned from the service operation.</typeparam>
public interface IServiceResultOf<out T>
{
  /// <summary>Gets the result data of the service execution, or null if the operation failed.</summary>
  T? Data { get; }

  /// <summary>Gets the collection of error messages that occurred during the service execution.</summary>
  IReadOnlyCollection<string> Errors { get; }

  /// <summary>Gets the exception that occurred during the service execution, if any.</summary>
  Exception? Exception { get; }

  /// <summary>Gets a value indicating whether the service call has succeeded.</summary>
  bool IsSuccess { get; }

  /// <summary>Gets the semantic status code of the service result.</summary>
  ResultTypeCode ResultType { get; }

  /// <summary>Converts the collection of error messages into a single formatted string.</summary>
  /// <param name="separator">The string to use as a separator between error messages. Defaults to "; ".</param>
  /// <returns>A string containing the formatted error messages, or an empty string if no errors exist.</returns>
  string ToErrorString(string separator = "; ");
}