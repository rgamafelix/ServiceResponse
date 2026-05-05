namespace RGamaFelix.ServiceResponse;

/// <summary>
///   Represents a placeholder type for service operations that do not return a meaningful data payload.
///   Acts similarly to the 'Unit' type in functional programming.
/// </summary>
public sealed class Void
{
  private Void()
  {
  }

  /// <summary>
  ///   Gets the singleton instance of the <see cref="Void" /> class.
  /// </summary>
  public static Void Value { get; } = new();
}