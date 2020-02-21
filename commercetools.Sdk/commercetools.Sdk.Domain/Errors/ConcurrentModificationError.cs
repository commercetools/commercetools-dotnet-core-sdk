namespace commercetools.Sdk.Domain.Errors
{
    [TypeMarker("ConcurrentModification")]
    public class ConcurrentModificationError : Error
    {
        public int CurrentVersion { get; set; }
    }
}