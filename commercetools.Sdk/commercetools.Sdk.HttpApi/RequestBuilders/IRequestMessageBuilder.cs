namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    /// <summary>
    /// This interface is used as marker interface to identify all the classes that are building request messages from commands.
    /// </summary>
    /// <remarks>
    /// It is used to register all implementing classes in DI.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1040", Justification = "used to register all implementing classes in DI")]
    public interface IRequestMessageBuilder
    {
    }
}
