using RGamaFelix.ServiceResponse;

namespace GamaFelixR.ServiceResponse.Tests;

[TestFixture]
public class ServiceResultTests
{
    [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.SuccessTypeCodes))]
    public void WhenCreateErrorResultWithSuccessResultTypeCode_ShouldThrowException(ResultTypeCode resultTypeCode)
    {
        Assert.That(() => ServiceResult.Fail("error", resultTypeCode), Throws.Exception.TypeOf<ArgumentException>());
    }

    [Test]
    public void WhenCreateResultWithException_ShouldReturnUnexpectedErrorTypeServiceResult()
    {
        var exception = new Exception("Error");
        var result = ServiceResult.Fail(exception);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ResultType, Is.EqualTo(ResultTypeCode.UnexpectedError));
            Assert.That(result.Errors.First(), Is.EqualTo(exception.Message));
            Assert.That(result.Exception, Is.EqualTo(exception));
        });
    }

    [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.ErrorTypeCodes))]
    public void WhenCreateResultWithError_ShouldReturnError(ResultTypeCode resultTypeCode)
    {
        var errors = new List<string>
        {
            "Error 1",
            "Error 2",
            "Error 3"
        };

        var result = ServiceResult.Fail(errors, resultTypeCode);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ResultType, Is.EqualTo(resultTypeCode));
            Assert.That(result.Errors, Is.EqualTo(errors));
        });
    }

    [Test]
    public void WhenCreateResultWithSuccessType_ShouldReturnSuccessServiceResult()
    {
        var result = ServiceResult.Success();
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess);
            Assert.That(result.ResultType, Is.EqualTo(ResultTypeCode.Ok));
        });
    }

    [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.ErrorTypeCodes))]
    public void ReturnsServiceResultWithErrorType(ResultTypeCode resultTypeCode)
    {
        var result = ServiceResult.Fail("error", resultTypeCode);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ResultType, Is.EqualTo(resultTypeCode));
        });
    }
}
