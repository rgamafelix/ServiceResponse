using FluentValidation.Results;
using RGamaFelix.ServiceResponse;
using RGamaFelix.ServiceResponse.FluentValidation;

namespace GamaFelixR.ServiceResponse.Tests;

public class FluentValidationHelperTests
{
  [Test]
  public void WhenValidationResultIsNull_ShouldThrowArgumentNullException()
  {
    Assert.That(() => ((ValidationResult)null!).ToServiceResult<string>(),
      Throws.ArgumentNullException);
  }

  [Test]
  public void WhenValidationResultIsValid_ShouldThrowInvalidOperationException()
  {
    var validationResult = new ValidationResult();

    Assert.That(() => validationResult.ToServiceResult<string>(),
      Throws.InvalidOperationException);
  }

  [Test]
  public void WhenValidationResultHasErrors_ShouldReturnInvalidDataServiceResult()
  {
    var validationResult = new ValidationResult(new[]
    {
      new ValidationFailure("Name", "Name is required"),
      new ValidationFailure("Email", "Email is invalid")
    });

    var result = validationResult.ToServiceResult<string>();

    Assert.Multiple(() =>
    {
      Assert.That(result.IsSuccess, Is.False);
      Assert.That(result.ResultType, Is.EqualTo(ResultTypeCode.InvalidData));
      Assert.That(result.Errors, Has.Count.EqualTo(2));
      Assert.That(result.Errors, Contains.Item("Name is required"));
      Assert.That(result.Errors, Contains.Item("Email is invalid"));
    });
  }

  [Test]
  public void WhenValidationResultHasSingleError_ShouldReturnServiceResultWithThatError()
  {
    var validationResult = new ValidationResult(new[]
    {
      new ValidationFailure("Name", "Name is required")
    });

    var result = validationResult.ToServiceResult<string>();

    Assert.Multiple(() =>
    {
      Assert.That(result.IsSuccess, Is.False);
      Assert.That(result.Errors, Has.Count.EqualTo(1));
      Assert.That(result.Errors.First(), Is.EqualTo("Name is required"));
    });
  }
}
