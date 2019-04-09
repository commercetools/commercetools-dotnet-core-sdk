namespace commercetools.Sdk.Domain
{
    public class CustomFieldsDraft : IDraft<CustomFields>
    {
        public IReferenceable<Type> Type { get; set; }
        public Fields Fields { get; set; }
    }
}
