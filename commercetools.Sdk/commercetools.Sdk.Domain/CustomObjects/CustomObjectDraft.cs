namespace commercetools.Sdk.Domain.CustomObjects
{
    public class CustomObjectDraft<T> : IDraft<CustomObject<T>>
    {
        public string Container { get; set; }

        public string Key { get; set; }

        public T Value { get; set; }

        public int? Version { get; set; }

    }
}
