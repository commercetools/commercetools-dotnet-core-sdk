namespace commercetools.Sdk.HttpApi.Domain
{
    using commercetools.Sdk.Domain;

    [TypeMarker("ConcurrentModification")]
    public class ConcurrentModificationError : Error
    {
        public int CurrentVersion { get; set; }
    }
}