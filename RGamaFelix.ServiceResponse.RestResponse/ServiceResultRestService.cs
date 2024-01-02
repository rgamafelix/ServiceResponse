using Microsoft.AspNetCore.Mvc;

namespace RGamaFelix.ServiceResponse.RestResponse;

public static class ServiceResultRestService
{
    public static IActionResult ReturnServiceResult(this IServiceResult response)
    {
        return response.ResultType.ToEnumValue() switch
        {
            ResultTypeCodeEnum.InvalidData => new BadRequestObjectResult(response.Errors),
            ResultTypeCodeEnum.Multiplicity => new ConflictObjectResult(response.Errors),
            ResultTypeCodeEnum.GenericError => new ObjectResult(response.Errors)
            {
                StatusCode = 500
            },
            ResultTypeCodeEnum.NotFound => new NotFoundObjectResult(response.Errors),
            ResultTypeCodeEnum.AuthenticationError => new UnauthorizedResult(),
            ResultTypeCodeEnum.UnexpectedError => new ObjectResult(response.Errors)
            {
                StatusCode = 500
            },
            ResultTypeCodeEnum.AuthorizationError => new ForbidResult(),
            ResultTypeCodeEnum.Ok => new NoContentResult(),
            ResultTypeCodeEnum.Created => throw new ArgumentException("Created response must provide a uri and/or data"),
            ResultTypeCodeEnum.Found => throw new ArgumentException("Found response must provide data"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static IActionResult ReturnServiceResult<T>(this IServiceResultOf<T> response, string? uri = null)
    {
        return response.ResultType.ToEnumValue() switch
        {
            ResultTypeCodeEnum.InvalidData => new BadRequestObjectResult(response.Errors),
            ResultTypeCodeEnum.Multiplicity => new ConflictObjectResult(response.Errors),
            ResultTypeCodeEnum.GenericError => new ObjectResult(response.Errors)
            {
                StatusCode = 500
            },
            ResultTypeCodeEnum.NotFound => new NotFoundObjectResult(response.Errors),
            ResultTypeCodeEnum.AuthenticationError => new UnauthorizedResult(),
            ResultTypeCodeEnum.UnexpectedError => new ObjectResult(response.Errors)
            {
                StatusCode = 500
            },
            ResultTypeCodeEnum.AuthorizationError => new ForbidResult(),
            ResultTypeCodeEnum.Ok => new OkObjectResult(response.Data),
            ResultTypeCodeEnum.Created => new CreatedResult(uri, response.Data),
            ResultTypeCodeEnum.Found => new OkObjectResult(response.Data),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
