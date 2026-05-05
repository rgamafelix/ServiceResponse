# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build all projects (all target frameworks)
dotnet build

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~ServiceResultOfTests"

# Pack all NuGet packages in Release mode
dotnet pack --configuration Release --output ./nupkgs

# Publish a package to nuget.org (API key in $NUGET_API_KEY)
dotnet nuget push ./nupkgs/<package>.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json
```

All projects target **net8.0;net9.0;net10.0** with nullable reference types and implicit usings enabled. The test project targets **net9.0** only.

## Architecture

Three publishable NuGet packages (all versioned together) share a common namespace root (`RGamaFelix.ServiceResponse`) and one test project:

### Core — `RGamaFelix.ServiceResponse`
- **`IServiceResultOf<T>`** — covariant interface; the return type service methods should expose.
- **`ServiceResultOf<T>`** — sole implementation. Constructor is private; callers use the static factory methods `Success(data, resultType)`, `Fail(errors, resultType)`, and `Fail(exception)`. `Success` throws `ArgumentNullException` on null data.
- **`ResultTypeCode`** — sealed record with a private constructor and a fixed set of static instances (`Ok`, `Found`, `Created`, `NotFound`, `InvalidData`, `GenericError`, `Multiplicity`, `AuthenticationError`, `AuthorizationError`, `UnexpectedError`). Adding a new code requires editing this file directly. Each code is tagged `IsSuccessCode = true/false`, which drives `ServiceResultOf<T>.IsSuccess`.
- **`Void`** — singleton used as `T` for service methods that return no meaningful data (`ServiceResultOf<Void>`).

### FluentValidation integration — `RGamaFelix.ServiceResponse.FluentValidation`
- Single static class `Helper` with one extension method: `validationResult.ToServiceResult<T>()`. Throws `InvalidOperationException` if called on a passing validation result.

### ASP.NET Core integration — `RGamaFelix.ServiceResponse.RestResponse`
- `ServiceResultRestService.ReturnServiceResult<T>(this IServiceResultOf<T>, string? uri, Options? options)` — extension method that switches on `ResultTypeCode` and returns the appropriate `IActionResult` (200, 201, 400, 401, 403, 404, 409, 500).
- `Options` — controls error response shape: `ErrorDetailLevel` (`ErrorMessages` returns actual errors; `DefaultErrorMessage` returns a single generic message) and `IncludeExceptionDetails` (appends exception details to `ResultErrorData.Details`). When `Options` is null the raw `Errors` collection is used as the response body (backward-compatible default).
- `ResultErrorData` — public record that is the response body when `Options` are provided. Shape: `{ Messages: string[], Details: string? }`.

### Tests — `RGamaFelix.ServiceResponse.Tests`
- NUnit 4 test project. Uses `[TestCaseSource]` with `ServiceResultTypeCodeProvider` to drive parameterized tests over all `ResultTypeCode` values. The provider uses reflection so it picks up new codes automatically.
- `ServiceResultRestServiceTests` — includes a reflection-driven test that calls `ReturnServiceResult` for every known `ResultTypeCode`, catching any missing switch arm at test time.

## Key invariants

- `ServiceResultOf<T>.Fail` rejects success codes; `ServiceResultOf<T>.Success` rejects error codes — both throw `ArgumentException`.
- `Fail(exception)` always produces `ResultTypeCode.UnexpectedError` and populates both `Errors` (with `exception.Message`) and `Exception`.
- `ReturnServiceResult` throws `ArgumentException` when `ResultTypeCode.Created` is used without a URI.
- `ReturnServiceResult` throws `ArgumentOutOfRangeException` for any `ResultTypeCode` not explicitly handled — adding a new code requires updating the switch in `ServiceResultRestService` (the exhaustive test in `ServiceResultRestServiceTests` will catch this).
