namespace commercetools.Sdk.Domain
{
    // TODO See if this attribute should be merged with TypeMarkerAttribute
    public class EndpointAttribute : System.Attribute
    {
        public EndpointAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }
    }
}