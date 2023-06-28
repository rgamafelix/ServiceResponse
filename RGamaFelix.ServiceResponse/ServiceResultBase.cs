using System.Collections.Immutable;
using System.Text;

namespace GamaFelixR;

public abstract class ServiceResultBase : IServiceResult
{
    private readonly List<string> _errors;

    protected private ServiceResultBase(IEnumerable<string>? errors, Exception? exception, ResultTypeCode errorType)
    {
        _errors = new List<string>(errors ?? new List<string>());
        ResultType = errorType;
        Exception = exception;
    }

    /// <summary>
    ///     Get the the list of errors occurred in the service execution
    /// </summary>
    public IReadOnlyCollection<string> Errors => _errors.ToImmutableArray();

    public Exception? Exception { get; }

    /// <summary>
    ///     Get a flag indicating if the service call has succeeded
    /// </summary>
    public bool IsSuccess => ResultType.IsSuccessCode;

    /// <summary>
    ///     General error type
    /// </summary>
    public ResultTypeCode ResultType { get; }

    /// <summary>
    ///     Returns a string describing the error.
    /// </summary>
    /// <returns></returns>
    public string ToErrorString()
    {
        var strBuilder = new StringBuilder();

        foreach (var error in _errors)
        {
            strBuilder.AppendLine($"{error};");
        }

        return strBuilder.ToString();
    }
}
