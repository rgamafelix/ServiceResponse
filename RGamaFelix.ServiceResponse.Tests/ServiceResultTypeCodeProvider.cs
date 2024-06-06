using RGamaFelix.ServiceResponse;

namespace GamaFelixR.ServiceResponse.Tests;

public class ServiceResultTypeCodeProvider
{
    public static IEnumerable<ResultTypeCode> SuccessTypeCodes => List.Where(x => x.IsSuccessCode);

    private static IEnumerable<ResultTypeCode> List =>
        new[]
        {
            ResultTypeCode.GenericError, ResultTypeCode.NotFound, ResultTypeCode.InvalidData,
            ResultTypeCode.Multiplicity, ResultTypeCode.AuthenticationError, ResultTypeCode.AuthorizationError,
            ResultTypeCode.UnexpectedError, ResultTypeCode.Created, ResultTypeCode.Ok, ResultTypeCode.Found
        };

    public static IEnumerable<ResultTypeCode> ErrorTypeCodes()
    {
        return List.Where(x => x.IsSuccessCode is false);
    }

    public static IEnumerable<ResultTypeCode> AllTypeCodes()
    {
        return List;
    }
}