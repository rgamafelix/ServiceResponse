# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build all projects
dotnet build

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~ServiceResultOfTests"

# Pack a NuGet package (e.g., core library)
dotnet pack RGamaFelix.ServiceResponse/RGamaFelix.ServiceResponse.csproj
```

All projects target **net9.0** with nullable reference types and implicit usings enabled.

## Architecture

Three publishable NuGet packages share a common namespace root (`RGamaFelix.ServiceResponse`) and one test project:

### Core — `RGamaFelix.ServiceResponse`
- **`IServiceResultOf<T>`** — covariant interface; the return type service methods should expose.
- **`ServiceResultOf<T>`** — sole implementation. Constructor is private; callers use the static factory methods `Success(data, resultType)`, `Fail(errors, resultType)`, and `Fail(exception)`. Includes implicit conversions to `T?` and `Exception?`.
- **`ResultTypeCode`** — sealed record with a private constructor and a fixed set of static instances (`Ok`, `Found`, `Created`, `NotFound`, `InvalidData`, `GenericError`, `Multiplicity`, `AuthenticationError`, `AuthorizationError`, `UnexpectedError`). Adding a new code requires editing this file directly. Each code is tagged `IsSuccessCode = true/false`, which drives `ServiceResultOf<T>.IsSuccess`.
- **`Void`** — singleton used as `T` for service methods that return no meaningful data (`ServiceResultOf<Void>`).

### FluentValidation integration — `RGamaFelix.ServiceResponse.FluentValidation`
- Single static class `Helper` with one extension method: `validationResult.ToServiceResult<T>()`. Throws `InvalidOperationException` if called on a passing validation result.

### ASP.NET Core integration — `RGamaFelix.ServiceResponse.RestResponse`
- `ServiceResultRestService.ReturnServiceResult<T>(this IServiceResultOf<T>, string? uri)` — extension method on `IServiceResultOf<T>` that switches on `ResultTypeCode` and returns the appropriate `IActionResult` (400, 401, 403, 404, 409, 500, 200, 201).
- `Options` — configures `ErrorDetailLevel` (`ErrorMessages` vs `DefaultErrorMessage`) and `IncludeExceptionDetails`.

### Tests — `RGamaFelix.ServiceResponse.Tests`
- NUnit 4 test project. Uses `[TestCaseSource]` with `ServiceResultTypeCodeProvider` to drive parameterized tests over all success/error `ResultTypeCode` values.

## Key invariants

- `ServiceResultOf<T>.Fail` rejects success codes; `ServiceResultOf<T>.Success` rejects error codes — both throw `ArgumentException`.
- `Fail(exception)` always produces `ResultTypeCode.UnexpectedError` and populates both `Errors` (with `exception.Message`) and `Exception`.
- `ReturnServiceResult` throws `ArgumentOutOfRangeException` for any `ResultTypeCode` not explicitly handled — adding a new code requires updating the switch in `ServiceResultRestService`.
