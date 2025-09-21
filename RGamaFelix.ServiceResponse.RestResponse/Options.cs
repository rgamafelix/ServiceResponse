namespace RGamaFelix.ServiceResponse.RestResponse;

public sealed class Options
{
  public string DefaultErrorMessage { get; set; } = "An error occurred while processing the request.";
  public ErrorDetailLevel ErrorDetailLevel { get; set; } = ErrorDetailLevel.ErrorMessages;
  public bool IncludeExceptionDetails { get; set; }
  public string? Uri { get; set; }
}