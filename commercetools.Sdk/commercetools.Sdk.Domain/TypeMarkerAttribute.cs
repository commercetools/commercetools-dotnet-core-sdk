namespace commercetools.Sdk.Domain
{
    public class TypeMarkerAttribute : System.Attribute
    {
        public TypeMarkerAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }
    }
}