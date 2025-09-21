# RGamaFelix.ServiceResponse.FluentValidation

Integration package that provides seamless conversion between FluentValidation results and ServiceResponse results.

## Overview

This package extends the [RGamaFelix.ServiceResponse](https://github.com/rgamafelix/ServiceResponse) library with helper
methods to easily convert FluentValidation `ValidationResult` objects into strongly-typed service results, maintaining
all validation error messages and providing consistent error handling.

## Installation

```bash 
  dotnet add package RGamaFelix.ServiceResponse.FluentValidation
```

## Dependencies

- **FluentValidation** (12.0.0+)
- **RGamaFelix.ServiceResponse** (3.0.0+)
- **.NET 9.0+**

## Features

- ✅ **Direct conversion** from `ValidationResult` to `IServiceResultOf<T>`
- ✅ **Automatic error message extraction** from validation failures
- ✅ **Type-safe results** with generic data types
- ✅ **Null-safe design** with comprehensive validation
- ✅ **Consistent error handling** with `ResultTypeCode.InvalidData`
- ✅ **Extension method pattern** for clean, fluent API

## Usage

### Basic Validation Conversion

```csharp
using FluentValidation; 
using RGamaFelix.ServiceResponse;
using RGamaFelix.ServiceResponse.FluentValidation;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserRequestValidator : AbstractValidator
{
    public CreateUserRequestValidator() 
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required") 
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");
    }
}

public class UserService
{
    public IServiceResultOfCreateUser(CreateUserRequest request)
    {
        var validator = new CreateUserRequestValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            // Convert validation errors to service result
            return validationResult.ToServiceResult<User>();
        }
        
        // Process valid request
        var user = new User { Name = request.Name, Email = request.Email };
        return ServiceResultOf<User>.Success(user, ResultTypeCode.Created);
    }
}
``` 

### Error Handling

```csharp
public async TaskCreateUser(CreateUserRequest request)
{
    var result = await userService.CreateUser(request);
    if (!result.IsSuccess)
    {
        // Validation errors are automatically included in the result
        return BadRequest(new
        {
            Errors = result.ErrorMessages,
            ResultCode = result.ResultTypeCode.Name
        });
    }
    
    return Created($"/api/users/{result.Data.Id}", result.Data);
}
```

### Multiple Validation Steps

```csharp
public IServiceResultOfProcessOrder(CreateOrderRequest request)
{
    // Validate basic order data
    var orderValidator = new CreateOrderRequestValidator(); 
    var orderValidation = orderValidator.Validate(request);
    if (!orderValidation.IsValid)
        return orderValidation.ToServiceResult<Order>();
    
    // Validate order items
    foreach (var item in request.Items)
    {
        var itemValidator = new OrderItemValidator();
        var itemValidation = itemValidator.Validate(item);
        if (!itemValidation.IsValid)
            return itemValidation.ToServiceResult<Order>();
    }
    
    // Process valid order
    var order = CreateOrder(request);
    return ServiceResultOf<Order>.Success(order, ResultTypeCode.Created);
}
``` 

## API Reference

### Extension Methods

#### `ToServiceResult<T>(ValidationResult)`

Converts a FluentValidation `ValidationResult` into a service result containing error messages.

**Parameters:**

- `validationResult` - The validation result to convert. Must not be null and must indicate a failed validation.

**Returns:**

- `IServiceResultOf<T>` - A service result with `ResultTypeCode.InvalidData` containing all validation error messages.

**Exceptions:**

- `ArgumentNullException` - Thrown when the validation result is null.
- `InvalidOperationException` - Thrown when the validation result indicates successful validation.

**Example:**

```csharp
var validationResult = validator.Validate(model);
if (!validationResult.IsValid)
{
    return validationResult.ToServiceResult();
}
```

## Error Message Processing

The extension method automatically:

1. **Filters out null errors** - Only processes validation errors with non-null error messages
2. **Extracts error messages** - Collects all `ErrorMessage` properties from failed validations
3. **Handles empty collections** - Returns an empty collection if no valid error messages are found
4. **Sets appropriate result code** - Always uses `ResultTypeCode.InvalidData` for validation failures

## Integration with ASP.NET Core

When combined with the RestResponse package:

```csharp
[ApiController]
[Route("api/[controller]")] 
public class UsersController : ControllerBase 
{
    [HttpPost]
        public async TaskCreateUser(CreateUserRequest request)
    {
        var result = await userService.CreateUser(request);
        // Automatic HTTP status code mapping:
        // ResultTypeCode.InvalidData -> 400 Bad Request
        // ResultTypeCode.Created -> 201 Created
        return result.ReturnServiceResult();
    }
}
```

## Version Information

- **Version**: 3.0.0
- **Target Framework**: .NET 9.0
- **Language**: C# 13.0 (latest major)
- **Nullable Reference Types**: Enabled

## License

This project is licensed under the MIT License - see
the [LICENSE](https://github.com/rgamafelix/ServiceResponse/blob/main/LICENSE) file for details.

## Repository

[https://github.com/rgamafelix/ServiceResponse](https://github.com/rgamafelix/ServiceResponse)

## Author

**Rodrigo Gama Felix** - [rodrigo@gamafelix.com.br](mailto:rodrigo@gamafelix.com.br)

## Related Packages

- [RGamaFelix.ServiceResponse](https://www.nuget.org/packages/RGamaFelix.ServiceResponse) - Core ServiceResponse library
- [RGamaFelix.ServiceResponse.RestResponse](https://www.nuget.org/packages/RGamaFelix.ServiceResponse.RestResponse)
- ASP.NET Core integration
