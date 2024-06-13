# ServiceResponse

This is a (very) simple library that implements a basic service response object. It is intended to be used in a service-oriented architecture, where the service response is the main way to communicate the result of a service operation.

## Installation

using dotnet cli:

```bash
dotnet add package RGamaFelix.ServiceResponse
```

using package manager:

```bash
Install-Package RGamaFelix.ServiceResponse
```


## Usage

```csharp
using RGamaFelix.ServiceResponse;

public class MyService
{
    public IServiceResultOf<int> MyMethod()
    {       
        try
        {
            // perform validation
            if (!valid)
            {
                return ServiceResultOf<int>.Fail("Validation failed", ResultTypeCode.InvalidData);
            }
            // do something
            var result = 42;
            return ServiceResultOf<int>.Success(result, ResultTypeCode.Ok);
        }
        catch (Exception ex)
        {
            return ServiceResultOf<int>.Fail(ex);
        }
    }
}
```

# FluentValidation integration

The library also provides an extension method to convert a FluentValidation.ValidationResult to a ServiceResponse object.

```csharp
using RGamaFelix.ServiceResponse;
using FluentValidation;
using Void = RGamaFelix.ServiceResponse.Void;

public class MyService
{
    public IServiceResultOf<Void> MyMethod()
    {       
        try
        {
            // perform validation
            var validator = new MyValidator();
            var validationResult = validator.Validate(new MyModel());
            if (!validationResult.IsValid)
            {
                return validationResult.ToServiceResult();
            }
            // do something
            return ServiceResultOf<Void>.Success(Void.Value, ResultTypeCode.Ok);
        }
        catch (Exception ex)
        {
            return ServiceResult.Fail(ex);
        }
    }
}
```

# RestResponse integration

The library also provides an extension method to convert a IActionResult to a ServiceResponse object.

```csharp
using RGamaFelix.ServiceResponse;
using Microsoft.AspNetCore.Mvc;

public interface IService
{
    IServiceResultOf<int> MyMethod();
}


public class MyController : ControllerBase
{
    private readonly IService _service;
    
    public MyController(IService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public IActionResult MyMethod()
    {       
        try
        {
            var result = _service.MyMethod();
            
            return result.ReturnServiceResult<int>();            
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToServiceResult());
        }
    }
}
```
