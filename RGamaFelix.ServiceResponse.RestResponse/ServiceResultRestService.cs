using Microsoft.AspNetCore.Mvc;

namespace RGamaFelix.ServiceResponse.RestResponse;

public static class ServiceResultRestService
{
  public static IActionResult ReturnServiceResult<T>(this IServiceResultOf<T> response, string? uri = null)
  {
    ArgumentNullException.ThrowIfNull(response);

    return response.ResultType switch
    {
      _ when response.ResultType == ResultTypeCode.InvalidData => new BadRequestObjectResult(response.Errors),
      _ when response.ResultType == ResultTypeCode.Multiplicity => new ConflictObjectResult(response.Errors),
      _ when response.ResultType == ResultTypeCode.GenericError => new ObjectResult(response.Errors)
      {
        StatusCode = 500
      },
      _ when response.ResultType == ResultTypeCode.NotFound => new NotFoundObjectResult(response.Errors),
      _ when response.ResultType == ResultTypeCode.AuthenticationError => new UnauthorizedResult(),
      _ when response.ResultType == ResultTypeCode.UnexpectedError => new ObjectResult(response.Errors)
      {
        StatusCode = 500
      },
      _ when response.ResultType == ResultTypeCode.AuthorizationError => new ForbidResult(),
      _ when response.ResultType == ResultTypeCode.Ok => new OkObjectResult(response.Data),
      _ when response.ResultType == ResultTypeCode.Created => CreateCreatedResult(uri, response.Data),
      _ when response.ResultType == ResultTypeCode.Found => new OkObjectResult(response.Data),
      _ => throw new ArgumentOutOfRangeException(nameof(response.ResultType), response.ResultType,
        "Unknown result type")
    };
  }

  private static IActionResult CreateCreatedResult<T>(string? uri, T? data)
  {
    return string.IsNullOrWhiteSpace(uri) ? new ObjectResult(data) { StatusCode = 201 } : new CreatedResult(uri, data);
  }
}