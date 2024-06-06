using Microsoft.AspNetCore.Mvc;

namespace RGamaFelix.ServiceResponse.RestResponse;

public static class ServiceResultRestService
{
    public static IActionResult ReturnServiceResult<T>(this IServiceResultOf<T> response, string? uri = null)
    {
        return response.ResultType.Name switch
        {
            nameof(ResultTypeCode.InvalidData) => new BadRequestObjectResult(response.Errors),
            nameof(ResultTypeCode.Multiplicity) => new ConflictObjectResult(response.Errors),
            nameof(ResultTypeCode.GenericError) => new ObjectResult(response.Errors) { StatusCode = 500 },
            nameof(ResultTypeCode.NotFound) => new NotFoundObjectResult(response.Errors),
            nameof(ResultTypeCode.AuthenticationError) => new UnauthorizedResult(),
            nameof(ResultTypeCode.UnexpectedError) => new ObjectResult(response.Errors) { StatusCode = 500 },
            nameof(ResultTypeCode.AuthorizationError) => new ForbidResult(),
            nameof(ResultTypeCode.Ok) => new OkObjectResult(response.Data),
            nameof(ResultTypeCode.Created) => new CreatedResult(uri, response.Data),
            nameof(ResultTypeCode.Found) => new OkObjectResult(response.Data),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
