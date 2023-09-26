using RGamaFelix.ServiceResponse;

namespace GamaFelixR.ServiceResponse.Tests;

public class ServiceResultTypeCodeProvider
{
    public static IEnumerable<ResultTypeCode> SuccessTypeCodes => ResultTypeCode.List.Where(x => x.IsSuccessCode);
    public static IEnumerable<ResultTypeCode> ErrorTypeCodes() => ResultTypeCode.List.Where(x => x.IsSuccessCode is false);
}
