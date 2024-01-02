using RGamaFelix.ServiceResponse;

namespace GamaFelixR.ServiceResponse.Tests;

public class ResultTypeCodeTests
{
    [TestCaseSource(typeof(ServiceResultTypeCodeProvider), nameof(ServiceResultTypeCodeProvider.AllTypeCodes))]
    public void WhenCastingToEnum_ShouldReturnEnumValue(ResultTypeCode resultTypeCode)
    {
        var enumValue = resultTypeCode.ToEnumValue();
        var expected = Enum.Parse<ResultTypeCodeEnum>(resultTypeCode.Name);
        Assert.That(enumValue, Is.EqualTo(expected));
    }
}
