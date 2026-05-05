using Microsoft.AspNetCore.Mvc;

namespace RGamaFelix.ServiceResponse.RestResponse;

/// <summary>
///   Provides extension methods to map <see cref="IServiceResultOf{T}" /> to ASP.NET Core <see cref="IActionResult" />.
/// </summary>
public static class ServiceResultRestService
{
  /// <summary>
  ///   Converts a service result into an appropriate <see cref="IActionResult" /> based on the <see cref="ResultTypeCode" />.
  /// </summary>
  /// <typeparam name="T">The type of the data in the service result.</typeparam>
  /// <param name="response">The service result to convert.</param>
  /// <param name="uri">The URI for 'Created' (201) responses. Required if <paramref name="response" /> has <see cref="ResultTypeCode.Created" />.</param>
  /// <param name="options">
  ///   Optional settings that control how error details are surfaced. When <c>null</c>, the raw
  ///   <see cref="IServiceResultOf{T}.Errors" /> collection is used as the response body (original behaviour).
  /// </param>
  /// <returns>An <see cref="IActionResult" /> representing the mapped result.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="response" /> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is null or whitespace for a 'Created' result.</exception>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the <see cref="ResultTypeCode" /> is not supported.</exception>
  /// <remarks>
  ///   Mappings:
  ///   <list type="bullet">
  ///     <item><see cref="ResultTypeCode.Ok" /> or <see cref="ResultTypeCode.Found" /> -> 200 OK (with data)</item>
  ///     <item><see cref="ResultTypeCode.Created" /> -> 201 Created (with data and Location header)</item>
  ///     <item><see cref="ResultTypeCode.InvalidData" /> -> 400 Bad Request (with errors)</item>
  ///     <item><see cref="ResultTypeCode.AuthenticationError" /> -> 401 Unauthorized</item>
  ///     <item><see cref="ResultTypeCode.AuthorizationError" /> -> 403 Forbidden</item>
  ///     <item><see cref="ResultTypeCode.NotFound" /> -> 404 Not Found (with errors)</item>
  ///     <item><see cref="ResultTypeCode.Multiplicity" /> -> 409 Conflict (with errors)</item>
  ///     <item><see cref="ResultTypeCode.GenericError" /> or <see cref="ResultTypeCode.UnexpectedError" /> -> 500 Internal Server Error (with errors)</item>
  ///   </list>
  /// </remarks>
  public static IActionResult ReturnServiceResult<T>(this IServiceResultOf<T> response, string? uri = null,
    Options? options = null)
  {
    ArgumentNullException.ThrowIfNull(response);

    return response.ResultType switch
    {
      _ when response.ResultType == ResultTypeCode.InvalidData =>
        new BadRequestObjectResult(BuildErrorBody(response, options)),
      _ when response.ResultType == ResultTypeCode.Multiplicity =>
        new ConflictObjectResult(BuildErrorBody(response, options)),
      _ when response.ResultType == ResultTypeCode.GenericError =>
        new ObjectResult(BuildErrorBody(response, options)) { StatusCode = 500 },
      _ when response.ResultType == ResultTypeCode.NotFound =>
        new NotFoundObjectResult(BuildErrorBody(response, options)),
      _ when response.ResultType == ResultTypeCode.AuthenticationError => new UnauthorizedResult(),
      _ when response.ResultType == ResultTypeCode.UnexpectedError =>
        new ObjectResult(BuildErrorBody(response, options)) { StatusCode = 500 },
      _ when response.ResultType == ResultTypeCode.AuthorizationError => new ForbidResult(),
      _ when response.ResultType == ResultTypeCode.Ok => new OkObjectResult(response.Data),
      _ when response.ResultType == ResultTypeCode.Created => CreateCreatedResult(uri, response.Data),
      _ when response.ResultType == ResultTypeCode.Found => new OkObjectResult(response.Data),
      _ => throw new ArgumentOutOfRangeException(nameof(response.ResultType), response.ResultType,
        "Unknown result type")
    };
  }

  private static object BuildErrorBody<T>(IServiceResultOf<T> response, Options? options)
  {
    if (options is null)
    {
      return response.Errors;
    }

    var messages = options.ErrorDetailLevel == ErrorDetailLevel.DefaultErrorMessage
      ? [options.DefaultErrorMessage]
      : response.Errors.ToArray();

    var errorData = new ResultErrorData(messages, null);

    if (options.IncludeExceptionDetails && response.Exception is not null)
    {
      errorData = errorData.AddDetails(response.Exception.ToString());
    }

    return errorData;
  }

  private static IActionResult CreateCreatedResult<T>(string? uri, T? data)
  {
    if (string.IsNullOrWhiteSpace(uri))
    {
      throw new ArgumentException("URI is required for 'Created' results.", nameof(uri));
    }

    return new CreatedResult(uri, data);
  }
}
