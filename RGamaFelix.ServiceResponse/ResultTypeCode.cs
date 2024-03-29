﻿using Ardalis.SmartEnum;

namespace RGamaFelix.ServiceResponse;

/// <summary>
///     Code of the result of a service execution
/// </summary>
public sealed class ResultTypeCode : SmartEnum<ResultTypeCode>
{
    private const bool _fail = false;
    private const bool _success = true;

    /// Indicates that the user is not authenticated.
    /// </summary>
    public static readonly ResultTypeCode AuthenticationError = new(nameof(AuthenticationError), 4, _fail);

    /// <summary>
    ///     Indicates that the user has been authenticated but is not authorized to perform the requested action.
    /// </summary>
    public static readonly ResultTypeCode AuthorizationError = new(nameof(AuthorizationError), 5, _fail);

    /// <summary>
    ///     Indicates that the resource was created successfully.
    /// </summary>
    public static readonly ResultTypeCode Created = new(nameof(Created), 7, _success);

    /// <summary>
    ///     Indicates that the requested resource was found.
    /// </summary>
    public static readonly ResultTypeCode Found = new(nameof(Found), 9, _success);

    /// <summary>
    ///     Indicates that an error occurred that is not covered by the other error codes.
    /// </summary>
    public static readonly ResultTypeCode GenericError = new(nameof(GenericError), 0, _fail);

    /// <summary>
    ///     Indicates that the request data has validation errors.
    /// </summary>
    public static readonly ResultTypeCode InvalidData = new(nameof(InvalidData), 2, _fail);

    /// <summary>
    ///     Indicates that more than one resource was found when only one was expected.
    /// </summary>
    public static readonly ResultTypeCode Multiplicity = new(nameof(Multiplicity), 3, _fail);

    /// <summary>
    ///     Indicates that the requested resource was not found.
    /// </summary>
    public static readonly ResultTypeCode NotFound = new(nameof(NotFound), 1, _fail);

    /// <summary>
    ///     Indicates that the request was successful.
    /// </summary>
    public static readonly ResultTypeCode Ok = new(nameof(Ok), 8, _success);

    /// <summary>
    ///     Indicates that an unexpected error occurred.
    /// </summary>
    /// <remarks>To be used in catch blocks. For normal 'unexpected erros' the <see cref="GenericError" /> is recomended</remarks>
    public static readonly ResultTypeCode UnexpectedError = new(nameof(UnexpectedError), 6, _fail);

    private ResultTypeCode(string name, int value, bool isSuccessCode) : base(name, value)
    {
        IsSuccessCode = isSuccessCode;
    }

    public ResultTypeCodeEnum ToEnumValue()
    {
        return Name switch
        {
            "GenericError" => ResultTypeCodeEnum.GenericError,
            "NotFound" => ResultTypeCodeEnum.NotFound,
            "InvalidData" => ResultTypeCodeEnum.InvalidData,
            "Multiplicity" => ResultTypeCodeEnum.Multiplicity,
            "AuthenticationError" => ResultTypeCodeEnum.AuthenticationError,
            "AuthorizationError" => ResultTypeCodeEnum.AuthorizationError,
            "UnexpectedError" => ResultTypeCodeEnum.UnexpectedError,
            "Created" => ResultTypeCodeEnum.Created,
            "Ok" => ResultTypeCodeEnum.Ok,
            "Found" => ResultTypeCodeEnum.Found,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool IsSuccessCode { get; }
}

public enum ResultTypeCodeEnum
{
    GenericError = 0,
    NotFound = 1,
    InvalidData = 2,
    Multiplicity = 3,
    AuthenticationError = 4,
    AuthorizationError = 5,
    UnexpectedError = 6,
    Created = 7,
    Ok = 8,
    Found = 9
}
