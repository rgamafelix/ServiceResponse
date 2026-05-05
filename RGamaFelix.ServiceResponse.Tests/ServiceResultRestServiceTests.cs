using Microsoft.AspNetCore.Mvc;
using RGamaFelix.ServiceResponse;
using RGamaFelix.ServiceResponse.RestResponse;

namespace GamaFelixR.ServiceResponse.Tests;

public class ServiceResultRestServiceTests
{
  [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.AllTypeCodes))]
  public void AllResultTypeCodesAreHandledByReturnServiceResult(ResultTypeCode code)
  {
    var result = code.IsSuccessCode
      ? ServiceResultOf<string>.Success("data", code)
      : ServiceResultOf<string>.Fail("error", code);

    var uri = code == ResultTypeCode.Created ? "/api/resource/1" : null;

    Assert.DoesNotThrow(() => result.ReturnServiceResult(uri));
  }

  [Test]
  public void WhenCreatedResultHasNoUri_ShouldThrowArgumentException()
  {
    var result = ServiceResultOf<string>.Success("data", ResultTypeCode.Created);

    Assert.That(() => result.ReturnServiceResult(), Throws.ArgumentException);
  }

  [Test]
  public void WhenNoOptionsProvided_ErrorBody_ShouldBeRawErrorsCollection()
  {
    var result = ServiceResultOf<string>.Fail("something went wrong", ResultTypeCode.InvalidData);

    var actionResult = (BadRequestObjectResult)result.ReturnServiceResult();

    Assert.That(actionResult.Value, Is.InstanceOf<IReadOnlyCollection<string>>());
  }

  [Test]
  public void WhenOptionsHasErrorMessages_ErrorBody_ShouldBeResultErrorDataWithActualErrors()
  {
    var errors = new[] { "Error A", "Error B" };
    var result = ServiceResultOf<string>.Fail(errors, ResultTypeCode.InvalidData);
    var options = new Options { ErrorDetailLevel = ErrorDetailLevel.ErrorMessages };

    var actionResult = (BadRequestObjectResult)result.ReturnServiceResult(options: options);
    var errorData = (ResultErrorData)actionResult.Value!;

    Assert.Multiple(() =>
    {
      Assert.That(errorData.Messages, Is.EqualTo(errors));
      Assert.That(errorData.Details, Is.Null);
    });
  }

  [Test]
  public void WhenOptionsHasDefaultErrorMessage_ErrorBody_ShouldBeResultErrorDataWithDefaultMessage()
  {
    var result = ServiceResultOf<string>.Fail("actual internal error", ResultTypeCode.InvalidData);
    var options = new Options { ErrorDetailLevel = ErrorDetailLevel.DefaultErrorMessage };

    var actionResult = (BadRequestObjectResult)result.ReturnServiceResult(options: options);
    var errorData = (ResultErrorData)actionResult.Value!;

    Assert.Multiple(() =>
    {
      Assert.That(errorData.Messages, Has.Length.EqualTo(1));
      Assert.That(errorData.Messages[0], Is.EqualTo(options.DefaultErrorMessage));
      Assert.That(errorData.Details, Is.Null);
    });
  }

  [Test]
  public void WhenOptionsHasIncludeExceptionDetails_AndExceptionExists_ShouldPopulateDetails()
  {
    var exception = new InvalidOperationException("boom");
    var result = ServiceResultOf<string>.Fail(exception);
    var options = new Options { IncludeExceptionDetails = true };

    var actionResult = (ObjectResult)result.ReturnServiceResult(options: options);
    var errorData = (ResultErrorData)actionResult.Value!;

    Assert.That(errorData.Details, Is.EqualTo(exception.ToString()));
  }

  [Test]
  public void WhenOptionsHasIncludeExceptionDetails_AndNoException_DetailsShouldBeNull()
  {
    var result = ServiceResultOf<string>.Fail("plain error", ResultTypeCode.GenericError);
    var options = new Options { IncludeExceptionDetails = true };

    var actionResult = (ObjectResult)result.ReturnServiceResult(options: options);
    var errorData = (ResultErrorData)actionResult.Value!;

    Assert.That(errorData.Details, Is.Null);
  }
}
