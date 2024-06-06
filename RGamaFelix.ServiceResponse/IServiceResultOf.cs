namespace RGamaFelix.ServiceResponse;

public interface IServiceResultOf<out T> : IServiceResult
{
    /// <summary>
    ///     Get the result of the service execution
    /// </summary>
    T? Data { get; }
}