namespace commercetools.Sdk.Domain.Errors
{
    /// <remarks>
    /// This class is only needed because of circular reference problems in deserialization.
    /// Most errors have the general error structure defined in the <see cref="Error"/> class and are represented by this class.
    /// </remarks>
    public class GeneralError : Error
    {
    }
}