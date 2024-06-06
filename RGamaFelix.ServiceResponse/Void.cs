namespace RGamaFelix.ServiceResponse;

/// <summary>
/// Dummy type used for methods that do not return a value
/// </summary>
public sealed class Void
{
    private Void()
    { }

    /// <summary>
    /// The only instance of the <see cref="Void"/> type
    /// </summary>
    public static Void Value { get; } = new();
}
