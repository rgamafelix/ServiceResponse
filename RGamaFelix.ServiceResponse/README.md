# RGamaFelix.ServiceResponse

A robust .NET library for handling service operation results with structured error handling and type safety.

## Overview

`RGamaFelix.ServiceResponse` provides a consistent way to handle the results of service operations, whether they succeed
or fail. It encapsulates data, errors, exceptions, and result status codes in a type-safe manner, making it easier to
build reliable applications with proper error handling.

## Features

- ✅ **Type-safe result handling** with generic support
- ✅ **Multiple error collection** with structured error messages
- ✅ **Exception wrapping** for unexpected errors
- ✅ **Result type codes** for categorizing different outcomes
- ✅ **Implicit conversions** for seamless integration
- ✅ **Rich debugging support** with comprehensive `ToString()` implementation
- ✅ **Interface-based design** for better testability and flexibility

## Installation

```bash
    dotnet add package RGamaFelix.ServiceResponse
```

## Quick Start

### Basic Usage

```csharp 
using RGamaFelix.ServiceResponse;
// Success case
var successResult = ServiceResultOf.Success("Hello World", ResultTypeCode.Success); 
if (successResult.IsSuccess)
{
    Console.WriteLine(successResult.Data); // "Hello World" 
}
// Failure case with single error 
var failureResult = ServiceResultOf.Fail("Something went wrong", ResultTypeCode.ValidationError);
if (!failureResult.IsSuccess)
{
    Console.WriteLine(failureResult.ToErrorString());
}
// Failure case with multiple errors 
var multipleErrors = new[] { "Error 1", "Error 2", "Error 3" };
var multiErrorResult = ServiceResultOf.Fail(multipleErrors, ResultTypeCode.ValidationError);
```

### Exception Handling

```csharp
try 
{
    // Some operation that might throw 
    throw new InvalidOperationException("Something unexpected happened"); 
} 
catch (Exception ex)
{
    var result = ServiceResultOf.Fail(ex); Console.WriteLine( "Exception: {result.Exception?.Message}"); Console.WriteLine("Errors: {result.ToErrorString()}");
} 
```

### Implicit Conversions

The library supports implicit conversions for seamless integration:

```csharp
// Convert to data type 
ServiceResultOfresult = ServiceResultOf .Success("data", ResultTypeCode.Success); 
string data = result; // Implicit conversion to string
// Convert to exception
ServiceResultOferrorResult = ServiceResultOf .Fail(new Exception("error")); Exception?exception = errorResult; // Implicit conversion to Exception
```

## API Reference

### IServiceResultOf\<T\>

The main interface that defines the contract for service results.

#### Properties

- `T? Data` - The data returned from a successful operation
- `IReadOnlyCollection<string> Errors` - Collection of error messages
- `Exception? Exception` - The exception that occurred (if any)
- `bool IsSuccess` - Indicates if the operation was successful
- `ResultTypeCode ResultType` - The type of result (success or error category)

#### Methods

- `string ToErrorString()` - Formats all errors into a single string

### ServiceResultOf\<T\>

The concrete implementation of `IServiceResultOf<T>`.

#### Static Factory Methods

**Success Methods:**

- `Success(T data, ResultTypeCode resultType)` - Creates a successful result

**Failure Methods:**

- `Fail(string error, ResultTypeCode resultType)` - Creates a failed result with single error
- `Fail(IEnumerable<string> errors, ResultTypeCode resultType)` - Creates a failed result with multiple errors
- `Fail(Exception exception)` - Creates a failed result from an exception

## Best Practices

### 1. Use Appropriate Result Type Codes

```csharp 
// For validation errors
var result = ServiceResultOf.Fail("Invalid email format", ResultTypeCode.ValidationError);
// For business logic violations
var result = ServiceResultOf.Fail("Insufficient inventory", ResultTypeCode.BusinessRuleViolation);
// For successful operations
var result = ServiceResultOf.Success(user, ResultTypeCode.Success);
``` 

### 2. Handle Both Success and Failure Cases

```csharp
var result = userService.GetUser(userId);
if (result.IsSuccess) 
{
    ProcessUser(result.Data);
}
else
{
    LogErrors(result.Errors); 
    if (result.Exception != null) 
    {
        LogException(result.Exception);
    } 
}
```

### 3. Use in Service Layer

```csharp
public class UserService 
{
    public IServiceResultOfCreateUser(CreateUserRequest request) 
    {
        // Validation 
        if (string.IsNullOrEmpty(request.Email))
        {
            return ServiceResultOf .Fail("Email is required", ResultTypeCode.ValidationError);
        }
        try
        {
            var user = new User(request.Email, request.Name);        
            _repository.Add(user);
            return ServiceResultOf<User>.Success(user, ResultTypeCode.Created);
        }
        catch (Exception ex)
        {
            return ServiceResultOf<User>.Fail(ex);
        }
    }
}
``` 

## Advanced Scenarios

### Working with Multiple Errors

```csharp
var errors = new List();
if (string.IsNullOrEmpty(request.Name)) errors.Add("Name is required");
if (string.IsNullOrEmpty(request.Email)) errors.Add("Email is required");
if (!IsValidEmail(request.Email)) errors.Add("Email format is invalid");
if (errors.Any())
{
    return ServiceResultOf.Fail(errors, ResultTypeCode.ValidationError);
}
```

### Chaining Operations

```csharp
public IServiceResultOfProcessData(int id)
{
    var fetchResult = _dataService.FetchData(id); 
    if (!fetchResult.IsSuccess)
    {
        return ServiceResultOf.Fail(fetchResult.Errors, fetchResult.ResultType);
    }
    try
    {
        var processed = ProcessRawData(fetchResult.Data);
        return ServiceResultOf<ProcessedData>.Success(processed, ResultTypeCode.Success);
    }

    catch (Exception ex)
    {
        return ServiceResultOf<ProcessedData>.Fail(ex);
    }
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

If you encounter any issues or have questions, please file an issue on the GitHub repository.
