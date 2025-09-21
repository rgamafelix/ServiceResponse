# RGamaFelix.ServiceResponse.RestResponse

ASP.NET Core integration package that provides automatic HTTP response mapping for ServiceResponse results.

## Overview

This package extends the [RGamaFelix.ServiceResponse](https://github.com/rgamafelix/ServiceResponse) library with
seamless ASP.NET Core integration, automatically converting service result types into appropriate HTTP status codes and
action results. Perfect for building RESTful APIs with consistent error handling and response formatting.

## Installation

```bash
  dotnet add package RGamaFelix.ServiceResponse.RestResponse
```

## Dependencies

- **Microsoft.AspNetCore.Mvc.Core** (2.3.0+)
- **Microsoft.AspNetCore.Mvc.Abstractions** (2.3.0+)
- **RGamaFelix.ServiceResponse** (3.0.0+)
- **.NET 9.0+**

## Features

- ✅ **Automatic HTTP mapping** from `ResultTypeCode` to HTTP status codes
- ✅ **Extension method integration** with fluent API design
- ✅ **Smart Created result handling** with optional location headers
- ✅ **Consistent error responses** with structured error data
- ✅ **Type-safe conversions** maintaining data integrity
- ✅ **Zero configuration** - works out of the box

## HTTP Status Code Mappings

| ResultTypeCode        | HTTP Status               | Action Result                    | Use Case                             |
|-----------------------|---------------------------|----------------------------------|--------------------------------------|
| `Ok`                  | 200 OK                    | `OkObjectResult`                 | General successful operation         |
| `Found`               | 200 OK                    | `OkObjectResult`                 | Resource successfully found          |
| `Created`             | 201 Created               | `CreatedResult` / `ObjectResult` | Resource successfully created        |
| `InvalidData`         | 400 Bad Request           | `BadRequestObjectResult`         | Validation or data format errors     |
| `NotFound`            | 404 Not Found             | `NotFoundObjectResult`           | Requested resource not found         |
| `AuthenticationError` | 401 Unauthorized          | `UnauthorizedResult`             | User not authenticated               |
| `AuthorizationError`  | 403 Forbidden             | `ForbidResult`                   | User not authorized                  |
| `Multiplicity`        | 409 Conflict              | `ConflictObjectResult`           | Multiple resources when one expected |
| `GenericError`        | 500 Internal Server Error | `ObjectResult`                   | General error condition              |
| `UnexpectedError`     | 500 Internal Server Error | `ObjectResult`                   | Unexpected exceptions                |

## Usage

### Basic Controller Integration

```csharp
using RGamaFelix.ServiceResponse.RestResponse;
using Microsoft.AspNetCore.Mvc;

[ApiController] 
[Route("api/[controller]")] 
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var result = _userService.GetUser(id);
        return result.ReturnServiceResult();
        
        // Automatically handles:
        // - Success: 200 OK with user data
        // - Not Found: 404 Not Found with error messages
        // - Validation Error: 400 Bad Request with error details
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserRequest request)
    {
        var result = _userService.CreateUser(request);
        return result.ReturnServiceResult($"/api/users/{result.Data?.Id}");
        
        // Automatically handles:
        // - Success: 201 Created with Location header
        // - Validation Error: 400 Bad Request
        // - Conflict: 409 Conflict
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, UpdateUserRequest request)
    {
        var result = _userService.UpdateUser(id, request);
        return result.ReturnServiceResult();
        // Automatically handles all result types
    }

    [HttpDelete("{id}")]
       public IActionResult DeleteUser(int id)
    {
        var result = _userService.DeleteUser(id);
        return result.ReturnServiceResult();
    }
}
```

### Advanced Usage with Custom Error Handling

```csharp 
[HttpPost]
public IActionResult CreateOrder(CreateOrderRequest request)
{
    var result = _orderService.CreateOrder(request);
    // The extension method handles all mapping automatically
    return result.ReturnServiceResult($"/api/orders/{result.Data?.Id}");        

    // Result mapping examples:
    // - ResultTypeCode.Created -> 201 Created with Location header
    // - ResultTypeCode.InvalidData -> 400 Bad Request with validation errors
    // - ResultTypeCode.AuthorizationError -> 403 Forbidden
    // - ResultTypeCode.UnexpectedError -> 500 Internal Server Error
}
```

### Error Response Format

Error responses automatically include structured error information:

```json
// 400 Bad Request example 
{
  "errors": "Invalid email address.",
  "details": "The email address is invalid."
}
// 404 Not Found example 
{
  "errors": "Requested user not found."
}
// 409 Conflict example 
{
  "errors": "User already exists."
}
// 500 Internal Server Error example 
{
  "errors": "An unexpected error occurred.
}
```

### Success Response Format

Success responses include the actual data:

```json
 // 200 OK example 
{
  "id": 123,
  "name": "John Doe",
  "email": "john@example.com",
  "createdAt": "2023-12-01T10:00:00Z"
}
// 201 Created example (with Location header)
{
  "id": 124,
  "name": "Jane Doe",
  "email": "jane@example.com",
  "createdAt": "2023-12-01T10:05:00Z"
}
```

## API Reference

### Extension Methods

#### `ReturnServiceResult<T>(IServiceResultOf<T>, string?)`

Converts a service result into an appropriate HTTP action result.

**Parameters:**

- `response` - The service result to convert (required)
- `uri` - Optional URI for Created responses (used when ResultTypeCode is Created)

**Returns:**

- `IActionResult` - Appropriate ASP.NET Core action result with correct HTTP status code

**Example:**

```csharp
var serviceResult = _userService.GetUser(id);
return serviceResult.ReturnServiceResult();
```

### Created Result Behavior

When `ResultTypeCode.Created` is returned:

- **With URI parameter**: Returns `CreatedResult` with Location header
- **Without URI parameter**: Returns `ObjectResult` with 201 status code

```csharp 
// With location header
return result.ReturnServiceResult("/api/users/123");
// Without location header
return result.ReturnServiceResult();
```
