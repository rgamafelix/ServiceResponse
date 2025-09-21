namespace RGamaFelix.ServiceResponse.RestResponse;

internal record ResultErrorData(string[] Messages, string? Details)
{
  public ResultErrorData AddDetails(string details)
  {
    return this with { Details = details };
  }
}