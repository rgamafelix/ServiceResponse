namespace RGamaFelix.ServiceResponse;

/// <summary>Represents a placeholder or dummy value for methods that do not return meaningful data.</summary>
public sealed class Void
{
  private Void()
  {
  }

  /// <summary>
  ///   Gets an instance of the <see cref="Void" /> class, representing a dummy value for methods that do not have a
  ///   meaningful return value.
  /// </summary>
  public static Void Value { get; } = new();
}
