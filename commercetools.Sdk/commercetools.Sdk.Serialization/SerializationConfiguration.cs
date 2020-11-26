namespace commercetools.Sdk.Serialization
{
    public class SerializationConfiguration : ISerializationConfiguration
    {
        public bool DeserializeDateAttributesAsString { get; set; }
        
        public bool DeserializeDateTimeAttributesAsString { get; set; }

        public SerializationConfiguration()
        {
            this.DeserializeDateAttributesAsString = false;
            this.DeserializeDateTimeAttributesAsString = false;
        }
    }
}