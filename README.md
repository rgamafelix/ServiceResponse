# ServiceResponse

A clean and extensible implementation of the Service Result pattern for .NET applications.

## Short Description

ServiceResponse is a .NET library that provides a standardized way to handle service operation results, encapsulating
both success and failure states with detailed error information and type-safe result codes.

## Detailed Description

ServiceResponse implements the Service Result pattern, a common architectural pattern used to standardize the return
values of service layer methods. Instead of throwing exceptions or returning null values for error conditions, this
library provides a consistent way to:

- **Encapsulate Success and Failure**: Every service operation returns a `ServiceResultOf<T>` that clearly indicates
  whether the operation succeeded or failed
- **Type-Safe Result Codes**: Predefined result codes like `NotFound`, `InvalidData`, `Created`, etc., provide semantic
  meaning to operation outcomes
- **Error Collection**: Multiple error messages can be collected and returned in a single result
- **Exception Handling**: Unexpected exceptions are captured and included in the result without breaking the call chain
- **Integration Support**: Additional packages provide seamless integration with FluentValidation and ASP.NET Core

The library consists of three main packages:

1. **RGamaFelix.ServiceResponse** - Core library with the base ServiceResult implementation
2. **RGamaFelix.ServiceResponse.FluentValidation** - Integration helpers for FluentValidation
3. **RGamaFelix.ServiceResponse.RestResponse** - ASP.NET Core integration for automatic HTTP response mapping

### Key Features

- ✅ **Type-safe results** with generic data types
- ✅ **Predefined result codes** covering common scenarios
- ✅ **Multiple error handling** with detailed error messages
- ✅ **Exception encapsulation** for unexpected errors
- ✅ **Implicit conversions** for easy data and exception access
- ✅ **FluentValidation integration** for validation error mapping
- ✅ **ASP.NET Core integration** for automatic HTTP status code mapping
- ✅ **Null-safe design** with nullable reference types
- ✅ **Comprehensive documentation** with XML comments

## Installation

### Core Package

```bash
  dotnet add package RGamaFelix.ServiceResponse
```

### FluentValidation Integration

```bash
  dotnet add package RGamaFelix.ServiceResponse.FluentValidation
```

### ASP.NET Core Integration

```bash
  dotnet add package RGamaFelix.ServiceResponse.RestResponse
```

## Usage Examples

### Basic Usage

```csharp
    using RGamaFelix.ServiceResponse;
    // Success result 
    public IServiceResultOfGetUser(int id)
    {
        var user = database.FindUser(id); 
        if (user == null) return ServiceResultOf.Fail("User not found", ResultTypeCode.NotFound);
        return ServiceResultOf<User>.Success(user, ResultTypeCode.Found);
    }
    // Consuming the result 
    var result = userService.GetUser(123); 
    if (result.IsSuccess)
    {
        Console.WriteLine("User: {result.Data.Name}");
    }
    else
    {
        Console.WriteLine("Errors: {result.ToErrorString()}"); 
    }
```

### Multiple Errors

```csharp
    public IServiceResultOfCreateUser(CreateUserRequest request)
    {
        var errors = new List ();
        if (string.IsNullOrEmpty(request.Name))
            errors.Add("Name is required");
        if (string.IsNullOrEmpty(request.Email))
            errors.Add("Email is required");
        if (errors.Any())
            return ServiceResultOf<User>.Fail(errors, ResultTypeCode.InvalidData);
        
        var user = new User(request.Name, request.Email);
        return ServiceResultOf<User>.Success(user, ResultTypeCode.Created);
    }
```

### Exception Handling

```csharp
    public IServiceResultOfProcessData(string input)
    {
        try
        {
            var result = ComplexOperation(input); 
            return ServiceResultOf.Success(result, ResultTypeCode.Ok);
        }
        catch (Exception ex)
        {
            return ServiceResultOf.Fail(ex);
        } 
    }
```

### FluentValidation Integration

```csharp
    using RGamaFelix.ServiceResponse.FluentValidation;
    using FluentValidation;
    
    public IServiceResultOfCreateUser(CreateUserRequest request) 
    {
        var validator = new CreateUserRequestValidator(); 
        var validationResult = validator.Validate(request);
        
        if (!validationResult.IsValid)
            return validationResult.ToErrorServiceResultOf<User>();
        
        var user = new User(request.Name, request.Email);
        return ServiceResultOf<User>.Success(user, ResultTypeCode.Created);
    }
```

### ASP.NET Core Integration

```csharp
    using RGamaFelix.ServiceResponse.RestResponse;
    
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")] 
        public IActionResult GetUser(int id)
        {
            var result = userService.GetUser(id);
            return result.ReturnServiceResult();
            // Automatically maps: 
            // - ResultTypeCode.Found -> 200 OK
            // - ResultTypeCode.NotFound -> 404 Not Found 
            // - ResultTypeCode.InvalidData -> 400 Bad Request 
            // etc. 
        }
        
        [HttpPost] 
        public IActionResult CreateUser(CreateUserRequest request)
        {
            var result = userService.CreateUser(request);
            return result.ReturnServiceResult("/api/users"); // ResultTypeCode.Created -> 201 Created with location header
        }
    }
```

### Implicit Conversions

```csharp
// Access data directly
IServiceResultOf<User> result = userService.GetUser(123);
User user = result; // Implicit conversion to User (null if failed)

// Access exception directly
Exception ex = result; // Implicit conversion to Exception (null if no exception)
```

### Available Result Type Codes

**Success Codes:**

-
    - General successful operation `ResultTypeCode.Ok`
-
    - Resource successfully created `ResultTypeCode.Created`
-
    - Resource successfully found `ResultTypeCode.Found`

**Error Codes:**

-
    - General error condition `ResultTypeCode.GenericError`
-
    - Requested resource not found `ResultTypeCode.NotFound`
-
    - Validation or data format errors `ResultTypeCode.InvalidData`
-
    - Multiple resources found when one expected `ResultTypeCode.Multiplicity`
-
    - User not authenticated `ResultTypeCode.AuthenticationError`
-
    - User not authorized `ResultTypeCode.AuthorizationError`
-
    - Unexpected exceptions (use in catch blocks) `ResultTypeCode.UnexpectedError`

## License

This project is licensed under the MIT License - see
the [LICENSE](https://github.com/rgamafelix/ServiceResponse/blob/main/LICENSE) file for details.

## Author

**Rodrigo Gama Felix** - [rodrigo@gamafelix.com.br]()

## Repository

[https://github.com/rgamafelix/ServiceResponse](https://github.com/rgamafelix/ServiceResponse)
