namespace RGamaFelix.ServiceResponse;

public interface IServiceResult
{
    /// <summary>
    ///     Get the the list of errors occurred in the service execution
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
