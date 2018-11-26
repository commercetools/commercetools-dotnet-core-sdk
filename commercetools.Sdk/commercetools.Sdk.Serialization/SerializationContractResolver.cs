namespace commercetools.Sdk.Serialization
{
    using System.Collections.Generic;

    public class SerializationContractResolver : CustomContractResolver
    {
        public SerializationContractResolver(IEnumerable<JsonConverterBase> registeredConverters) : base(registeredConverters)
        { }

        protected override SerializerType SerializerType { get => SerializerType.Serialization; }
    }
}