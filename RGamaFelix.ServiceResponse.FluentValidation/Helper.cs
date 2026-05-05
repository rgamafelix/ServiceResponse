using FluentValidation.Results;

namespace RGamaFelix.ServiceResponse.FluentValidation;

/// <summary>
///   Provides extension methods to integrate FluentValidation with the ServiceResponse library.
/// </summary>
public static class Helper
{
  private const string InvalidValidationResultMessage =
    "Success validation result cannot be converted to error service result";

  /// <summary>
  ///   Converts a failed FluentValidation <see cref="ValidationResult" /> into a service result.
  /// </summary>
  /// <typeparam name="T">The type of the data in the service result.</typeparam>
  /// <param name="validationResult">The validation result to convert. Must indicate a failure.</param>
  /// <returns>
  ///   An <see cref="IServiceResultOf{T}" /> with <see cref="ResultTypeCode.InvalidData" /> containing the error messages from the validation result.
  /// </returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="validationResult" /> is null.</exception>
  /// <exception cref="InvalidOperationException">Thrown when <paramref name="validationResult" /> indicates a successful validation.</exception>
  public static IServiceResultOf<T> ToServiceResult<T>(this ValidationResult validationResult)
  {
    ArgumentNullException.ThrowIfNull(validationResult);

    if (validationResult.IsValid)
    {
      throw new InvalidOperationException(InvalidValidationResultMessage);
    }

    var errorMessages = validationResult.Errors.Where(e => e.ErrorMessage != null).Select(e => e.ErrorMessage);

    return ServiceResultOf<T>.Fail(errorMessages, ResultTypeCode.InvalidData);
  }
}
