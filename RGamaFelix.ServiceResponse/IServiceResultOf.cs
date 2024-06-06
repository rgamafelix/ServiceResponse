namespace RGamaFelix.ServiceResponse;

public interface IServiceResultOf<out T>
{
    /// <summary>
    ///     Get the result of the service execution
    /// </summary>
    T? Data { get; }

    /// <summary>
    ///     Get the list of errors occurred in the service execution
    /// </summary>
    IReadOnlyCollection<string> Errors { get; }

    Exception? Exception { get; }

    /// <summary>
    ///     Get a flag indicating if the service call has succeeded
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    ///     General error type
    /// </summary>
    ResultTypeCode ResultType { get; }

    /// <summary>
    ///     When implemented in a derived class, returns a string describing the error.
    /// </summary>
    /// <returns></returns>
    string ToErrorString();
}
