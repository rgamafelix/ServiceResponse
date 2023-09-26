using GamaFelixR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace RGamaFelix.ServiceResponse.RestResponse;

public static class ServiceResultRestService
{
    public static IActionResult ReturnServiceResult(this IServiceResult response)
    {
        return response.ResultType.Name switch
        {
            "InvalidData" => new BadRequestObjectResult(response.Errors),
            "Multiplicity" => new ConflictObjectResult(response.Errors),
            "GenericError" => new ObjectResult(response.Errors)
            {
                StatusCode = 500
            },
            "NotFound" => new NotFoundObjectResult(response.Errors),
            "AuthenticationError" => new UnauthorizedResult(),
            "UnexpectedError" => new ObjectResult(response.Errors)
            {
                StatusCode = 500
            },
            "AuthorizationError" => new ForbidResult(),
            "Ok" => new NoContentResult(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static IActionResult ReturnServiceResult<T>(this IServiceResultOf<T> response, string? uri = null)
    {
        return response.ResultType.Name switch
        {
            "InvalidData" => new BadRequestObjectResult(response.Errors),
            "Multiplicity" => new ConflictObjectResult(response.Errors),
            "GenericError" => new ObjectResult(response.Errors)
            {
                StatusCode = 500
            },
            "NotFound" => new NotFoundObjectResult(response.Errors),
            "AuthenticationError" => new UnauthorizedResult(),
            "UnexpectedError" => new ObjectResult(response.Errors)
            {
                StatusCode = 500
            },
            "AuthorizationError" => new ForbidResult(),
            "Ok" => new OkObjectResult(response.Data),
            "Created" => new CreatedResult(uri, response.Data),
            "Found" => new OkObjectResult(response.Data),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
