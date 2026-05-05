namespace RGamaFelix.ServiceResponse.RestResponse;

/// <summary>
///   Defines the options for configuring how service results are mapped to REST responses.
/// </summary>
public sealed class Options
{
  /// <summary>Gets or sets the default error message to use when detailed errors are hidden.</summary>
  public string DefaultErrorMessage { get; set; } = "An error occurred while processing the request.";

  /// <summary>Gets or sets the level of detail to include in error responses.</summary>
  public ErrorDetailLevel ErrorDetailLevel { get; set; } = ErrorDetailLevel.ErrorMessages;

  /// <summary>Gets or sets a value indicating whether to include full exception details in the response.</summary>
  public bool IncludeExceptionDetails { get; set; }
}
