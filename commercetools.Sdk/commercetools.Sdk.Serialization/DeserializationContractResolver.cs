namespace commercetools.Sdk.Serialization
{
    using System.Collections.Generic;

    public class DeserializationContractResolver : CustomContractResolver
    {
        public DeserializationContractResolver(IEnumerable<JsonConverterBase> registeredConverters) : base(registeredConverters)
        { }

        protected override SerializerType SerializerType { get => SerializerType.Deserialization; }
    }
}
