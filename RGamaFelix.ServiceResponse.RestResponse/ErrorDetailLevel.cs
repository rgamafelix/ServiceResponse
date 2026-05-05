namespace RGamaFelix.ServiceResponse.RestResponse;

/// <summary>
///   Specifies the level of detail included in error results returned by the REST integration.
/// </summary>
public enum ErrorDetailLevel
{
  /// <summary>Include all error messages from the service result.</summary>
  ErrorMessages = 0,

  /// <summary>Include only the default error message, hiding specific details.</summary>
  DefaultErrorMessage = 1
}