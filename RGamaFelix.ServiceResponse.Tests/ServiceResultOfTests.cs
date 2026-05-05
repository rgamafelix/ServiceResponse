using RGamaFelix.ServiceResponse;

namespace GamaFelixR.ServiceResponse.Tests;

public class ServiceResultOfTests
{
  private TestObject _testObject;

  [OneTimeSetUp]
  public void Setup()
  {
    _testObject = new TestObject { Name = "Name", Value = 500 };
  }

  [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.SuccessTypeCodes))]
  public void WhenCreateErrorResultWithSuccessResultTypeCode_ShouldThrowException(ResultTypeCode resultTypeCode)
  {
    Assert.That(() => ServiceResultOf<TestObject>.Fail("error", resultTypeCode),
      Throws.Exception.TypeOf<ArgumentException>());
  }

  [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.ErrorTypeCodes))]
  public void WhenCreateResultWithError_ShouldReturnError(ResultTypeCode resultTypeCode)
  {
    var errors = new List<string> { "Error 1", "Error 2", "Error 3" };
    var result = ServiceResultOf<TestObject>.Fail(errors, resultTypeCode);

    Assert.Multiple(() =>
    {
      Assert.That(result.IsSuccess, Is.False);
      Assert.That(result.ResultType, Is.EqualTo(resultTypeCode));
      Assert.That(result.Exception, Is.Null);
      Assert.That(result.Data, Is.Null);
      Assert.That(result.Errors, Is.EqualTo(errors));
    });
  }

  [Test]
  public void WhenCreateResultWithException_ShouldReturnUnexpectedErrorTypeServiceResult()
  {
    var exception = new Exception("Error");
    var result = ServiceResultOf<TestObject>.Fail(exception);

    Assert.Multiple(() =>
    {
      Assert.That(result.IsSuccess, Is.False);
      Assert.That(result.ResultType, Is.EqualTo(ResultTypeCode.UnexpectedError));
      Assert.That(result.Errors.First(), Is.EqualTo(exception.Message));
      Assert.That(result.Data, Is.Null);
      Assert.That(result.Exception, Is.EqualTo(exception));
    });
  }

  [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.SuccessTypeCodes))]
  public void WhenCreateResultWithSuccessType_ShouldReturnSuccessServiceResult(ResultTypeCode resultTypeCode)
  {
    var result = ServiceResultOf<TestObject>.Success(_testObject, resultTypeCode);

    Assert.Multiple(() =>
    {
      Assert.That(result.IsSuccess);
      Assert.That(result.Data, Is.EqualTo(_testObject));
      Assert.That(result.Exception, Is.Null);
      Assert.That(result.Errors, Is.Empty);
      Assert.That(result.ResultType, Is.EqualTo(resultTypeCode));
    });
  }

  [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.ErrorTypeCodes))]
  public void WhenCreateSuccessResultWithErrorResultTypeCode_ShouldThrowException(ResultTypeCode resultTypeCode)
  {
    Assert.That(() => ServiceResultOf<TestObject>.Success(_testObject, resultTypeCode),
      Throws.Exception.TypeOf<ArgumentException>());
  }

  [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.ErrorTypeCodes))]
  public void WhenCreateResultWithSingleError_ShouldReturnSingleError(ResultTypeCode resultTypeCode)
  {
    var result = ServiceResultOf<TestObject>.Fail("Single error", resultTypeCode);

    Assert.Multiple(() =>
    {
      Assert.That(result.IsSuccess, Is.False);
      Assert.That(result.Errors, Has.Count.EqualTo(1));
      Assert.That(result.Errors.First(), Is.EqualTo("Single error"));
      Assert.That(result.ResultType, Is.EqualTo(resultTypeCode));
    });
  }

  [Test]
  public void WhenCreateSuccessResultWithVoid_ShouldReturnSuccess()
  {
    var result = ServiceResultOf<RGamaFelix.ServiceResponse.Void>.Success(
      RGamaFelix.ServiceResponse.Void.Value, ResultTypeCode.Ok);

    Assert.Multiple(() =>
    {
      Assert.That(result.IsSuccess);
      Assert.That(result.Data, Is.EqualTo(RGamaFelix.ServiceResponse.Void.Value));
      Assert.That(result.Errors, Is.Empty);
      Assert.That(result.Exception, Is.Null);
    });
  }

  [Test]
  public void WhenToErrorString_WithNoErrors_ShouldReturnEmpty()
  {
    var result = ServiceResultOf<TestObject>.Success(_testObject, ResultTypeCode.Ok);

    Assert.That(result.ToErrorString(), Is.Empty);
  }

  [Test]
  public void WhenToErrorString_WithSingleError_ShouldReturnError()
  {
    var result = ServiceResultOf<TestObject>.Fail("Something went wrong", ResultTypeCode.GenericError);

    Assert.That(result.ToErrorString(), Is.EqualTo("Something went wrong"));
  }

  [Test]
  public void WhenToErrorString_WithMultipleErrors_ShouldUseDefaultSeparator()
  {
    var errors = new[] { "Error A", "Error B", "Error C" };
    var result = ServiceResultOf<TestObject>.Fail(errors, ResultTypeCode.InvalidData);

    Assert.That(result.ToErrorString(), Is.EqualTo("Error A; Error B; Error C"));
  }

  [Test]
  public void WhenToErrorString_WithCustomSeparator_ShouldUseCustomSeparator()
  {
    var errors = new[] { "Error A", "Error B" };
    var result = ServiceResultOf<TestObject>.Fail(errors, ResultTypeCode.InvalidData);

    Assert.That(result.ToErrorString(" | "), Is.EqualTo("Error A | Error B"));
  }

  private class TestObject
  {
    public string? Name { get; set; }
    public int Value { get; set; }
  }
}