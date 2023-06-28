namespace GamaFelixR.ServiceResponse.Tests;

public class ServiceResultOfTests
{
    private TestObject testObject;

    [OneTimeSetUp]
    public void Setup()
    {
        testObject = new TestObject
        {
            Name = "Name",
            Value = 500
        };
    }

    [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.SuccessTypeCodes))]
    public void WhenCreateErrorResultWithSuccessResultTypeCode_ShouldThrowException(ResultTypeCode resultTypeCode)
    {
        Assert.That(() => ServiceResultOf<TestObject>.Fail("error", resultTypeCode), Throws.Exception.TypeOf<ArgumentException>());
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
        var result = ServiceResultOf<TestObject>.Success(testObject, resultTypeCode);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess);
            Assert.That(result.Data, Is.EqualTo(testObject));
            Assert.That(result.Exception, Is.Null);
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.ResultType, Is.EqualTo(resultTypeCode));
        });
    }

    [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.ErrorTypeCodes))]
    public void WhenCreateSuccessResultWithErrorResultTypeCode_ShouldThrowException(ResultTypeCode resultTypeCode)
    {
        Assert.That(() => ServiceResultOf<TestObject>.Success(testObject, resultTypeCode), Throws.Exception.TypeOf<ArgumentException>());
    }

    private class TestObject
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
