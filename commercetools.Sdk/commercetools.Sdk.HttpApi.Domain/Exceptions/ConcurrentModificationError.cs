namespace commercetools.Sdk.HttpApi.Domain
{
    [StatusCode("ConcurrentModification")]
    public class ConcurrentModificationError : Error
    {
        public int CurrentVersion { get; set; }
    }
}