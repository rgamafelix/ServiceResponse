namespace RGamaFelix.ServiceResponse.RestResponse;

/// <summary>Represents the error body returned in REST responses when <see cref="Options" /> are provided.</summary>
public record ResultErrorData(string[] Messages, string? Details)
{
  internal ResultErrorData AddDetails(string details)
  {
    return this with { Details = details };
  }
}