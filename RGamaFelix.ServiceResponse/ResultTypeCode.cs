namespace RGamaFelix.ServiceResponse;

/// <summary>
///   Represents the semantic status code of a service result.
///   <para>
///     Note: When adding new codes, ensure they are also supported in integration packages
///     (e.g., RGamaFelix.ServiceResponse.RestResponse) to avoid runtime errors.
///   </para>
/// </summary>
public sealed record ResultTypeCode
{
  /// <summary>Indicates a general failure that is not covered by more specific error codes.</summary>
  public static readonly ResultTypeCode GenericError = new(nameof(GenericError), 0, Fail);

  /// <summary>Indicates that the requested resource was not found.</summary>
  public static readonly ResultTypeCode NotFound = new(nameof(NotFound), 1, Fail);

  /// <summary>Indicates that the request data failed validation or is in an invalid format.</summary>
  public static readonly ResultTypeCode InvalidData = new(nameof(InvalidData), 2, Fail);

  /// <summary>Indicates that the request conflicts with the current state of the resource, such as a uniqueness or duplicate constraint violation.</summary>
  public static readonly ResultTypeCode Multiplicity = new(nameof(Multiplicity), 3, Fail);

  /// <summary>Indicates that the user is not authenticated.</summary>
  public static readonly ResultTypeCode AuthenticationError = new(nameof(AuthenticationError), 4, Fail);

  /// <summary>Indicates that the user is authenticated but does not have permission to perform the action.</summary>
  public static readonly ResultTypeCode AuthorizationError = new(nameof(AuthorizationError), 5, Fail);

  /// <summary>Indicates that an unexpected exception occurred during execution.</summary>
  public static readonly ResultTypeCode UnexpectedError = new(nameof(UnexpectedError), 6, Fail);

  /// <summary>Indicates that the resource was successfully created.</summary>
  public static readonly ResultTypeCode Created = new(nameof(Created), 7, Success);

  /// <summary>Indicates that the requested resource was successfully found.</summary>
  public static readonly ResultTypeCode Found = new(nameof(Found), 9, Success);

  /// <summary>Indicates that the operation completed successfully.</summary>
  public static readonly ResultTypeCode Ok = new(nameof(Ok), 8, Success);

  private const bool Fail = false;
  private const bool Success = true;

  private ResultTypeCode(string name, int value, bool isSuccessCode)
  {
    Name = name;
    Value = value;
    IsSuccessCode = isSuccessCode;
  }

  /// <summary>Gets a value indicating whether this code represents a successful operation.</summary>
  public bool IsSuccessCode { get; }

  /// <summary>Gets the name of the result type code.</summary>
  public string Name { get; }

  /// <summary>Gets the underlying integer value of the result type code.</summary>
  public int Value { get; }
}
