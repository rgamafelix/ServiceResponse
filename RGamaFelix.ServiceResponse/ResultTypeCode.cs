﻿using Ardalis.SmartEnum;

namespace GamaFelixR;

/// <summary>
///     Code of the result of a service execution
/// </summary>
public sealed class ResultTypeCode : SmartEnum<ResultTypeCode>
{
    private const bool fail = false;
    private const bool success = true;

    /// <summary>
    ///     Indicates that the user is not authenticated.
    /// </summary>
    public static readonly ResultTypeCode AuthenticationError = new(nameof(AuthenticationError), 4, fail);

    /// <summary>
    ///     Indicates that the user has been authenticated but is not authorized to perform the requested action.
    /// </summary>
    public static readonly ResultTypeCode AuthorizationError = new(nameof(AuthorizationError), 5, fail);

    /// <summary>
    ///     Indicates that the resource was created successfully.
    /// </summary>
    public static readonly ResultTypeCode Created = new(nameof(Created), 7, success);

    /// <summary>
    ///     Indicates that the requested resource was found.
    /// </summary>
    public static readonly ResultTypeCode Found = new(nameof(Found), 9, success);

    /// <summary>
    ///     Indicates that an error occurred that is not covered by the other error codes.
    /// </summary>
    public static readonly ResultTypeCode GenericError = new(nameof(GenericError), 0, fail);

    /// <summary>
    ///     Indicates that the request data has validation errors.
    /// </summary>
    public static readonly ResultTypeCode InvalidData = new(nameof(InvalidData), 2, fail);

    /// <summary>
    ///     Indicates that more than one resource was found when only one was expected.
    /// </summary>
    public static readonly ResultTypeCode Multiplicity = new(nameof(Multiplicity), 3, fail);

    /// <summary>
    ///     Indicates that the requested resource was not found.
    /// </summary>
    public static readonly ResultTypeCode NotFound = new(nameof(NotFound), 1, fail);

    /// <summary>
    ///     Indicates that the request was successful.
    /// </summary>
    public static readonly ResultTypeCode Ok = new(nameof(Ok), 8, success);

    /// <summary>
    ///     Indicates that an unexpected error occurred.
    /// </summary>
    /// <remarks>To be used in catch blocks. For normal 'unexpected erros' the <see cref="GenericError" /> is recomended</remarks>
    public static readonly ResultTypeCode UnexpectedError = new(nameof(UnexpectedError), 6, fail);

    private ResultTypeCode(string name, int value, bool isSuccessCode) : base(name, value)
    {
        IsSuccessCode = isSuccessCode;
    }

    public bool IsSuccessCode { get; }
}