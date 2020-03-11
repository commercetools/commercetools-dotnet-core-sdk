namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class Attribute<T> : Attribute, IAttribute
    {
        public T Value { get; set; }

        public object JsonValue { get; set; }

        public bool ShouldSerializeJsonValue()
        {
            return false;
        }

        protected void CloneProperties(Attribute<T> attribute)
        {
            this.Value = attribute.Value;
            this.Name = attribute.Name;
            this.JsonValue = attribute.JsonValue;
        }
    }
}
