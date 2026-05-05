using System.Reflection;
using RGamaFelix.ServiceResponse;

namespace GamaFelixR.ServiceResponse.Tests;

public class ServiceResultTypeCodeProvider
{
  public static IEnumerable<ResultTypeCode> SuccessTypeCodes()
  {
    return AllTypeCodes().Where(x => x.IsSuccessCode);
  }

  public static IEnumerable<ResultTypeCode> AllTypeCodes()
  {
    return typeof(ResultTypeCode).GetFields(BindingFlags.Public | BindingFlags.Static)
      .Where(f => f.FieldType == typeof(ResultTypeCode))
      .Select(f => (ResultTypeCode)f.GetValue(null)!);
  }

  public static IEnumerable<ResultTypeCode> ErrorTypeCodes()
  {
    return AllTypeCodes().Where(x => x.IsSuccessCode is false);
  }
}
