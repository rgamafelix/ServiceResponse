using FluentValidation.Results;

namespace RGamaFelix.ServiceResponse.FluentValidation;

/// <summary>
/// Provides helper methods to work with FluentValidation results and convert them to service results.
/// </summary>
public static class Helper
{
  private const string InvalidValidationResultMessage =
    "Success validation result cannot be converted to error service result";

  /// <summary>
  /// Converts a FluentValidation <see cref="ValidationResult"/> into a service result containing error messages.
  /// </summary>
  /// <typeparam name="T">The type of the result data in the service result.</typeparam>
  /// <param name="validationResult">The validation result to convert. Must not be null, and must indicate a failed validation.</param>
  /// <returns>An <see cref="IServiceResultOf{T}"/> containing the error messages from the validation result.</returns>
  /// <exception cref="ArgumentNullException">Thrown when the <paramref name="validationResult"/> is null.</exception>
  /// <exception cref="InvalidOperationException">Thrown when the <paramref name="validationResult"/> indicates a successful validation.</exception>
  public static IServiceResultOf<T> ToServiceResult<T>(this ValidationResult validationResult)
  {
    ArgumentNullException.ThrowIfNull(validationResult);

    if (validationResult.IsValid)
    {
      throw new InvalidOperationException(InvalidValidationResultMessage);
    }

    var errorMessages = validationResult.Errors?.Where(e => e?.ErrorMessage != null)
      .Select(e => e.ErrorMessage) ?? [];

    return ServiceResultOf<T>.Fail(errorMessages, ResultTypeCode.InvalidData);
  }
}