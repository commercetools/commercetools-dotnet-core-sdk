namespace commercetools.Sdk.Serialization
{
    public interface ISerializationConfiguration
    {
        bool DeserializeDateAttributesAsString { get; }
        
        bool DeserializeDateTimeAttributesAsString { get; }
    }
}