namespace commercetools.Sdk.HttpApi.Domain
{
    [ErrorType("ConcurrentModification")]
    public class ConcurrentModificationError : Error
    {
        public int CurrentVersion { get; set; }
    }
}