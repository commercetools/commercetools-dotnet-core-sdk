namespace commercetools.Sdk.Domain
{
    public class Attribute<T> : Attribute
    {
        public string Name { get; set; }
        public T Value { get; set; }
    }
}