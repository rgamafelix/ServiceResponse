using FluentValidation.Results;

namespace RGamaFelix.ServiceResponse.FluentValidation;

public static class Helper
{
    public static IServiceResultOf<T> ToErrorServiceResultOf<T>(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
        {
            throw new ArgumentException("Success validation result cannot be converted to error service result");
        }

        return ServiceResultOf<T>.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
            ResultTypeCode.InvalidData);
    }
}
