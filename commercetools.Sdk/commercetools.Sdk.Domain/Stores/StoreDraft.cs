namespace commercetools.Sdk.Domain.Stores
{
    public class StoreDraft : IDraft<Store>
    {
        public string Key { get; set; }
        public LocalizedString Name { get; set; }
    }
}
