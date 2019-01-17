namespace commercetools.Sdk.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Collections.Generic;
    using System.Linq;
    using Type = System.Type;

    internal abstract class CustomContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly IEnumerable<JsonConverterBase> registeredConverters;

        protected abstract SerializerType SerializerType { get; }

        public CustomContractResolver(IEnumerable<JsonConverterBase> registeredConverters)
        {
            this.registeredConverters = registeredConverters;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            // This method will be run only once per type and the return type will be cached.
            // https://www.newtonsoft.com/json/help/html/Performance.htm
            JsonContract contract = base.CreateContract(objectType);

            JsonConverter jsonConverter = registeredConverters.Where(c => c.CanConvert(objectType) && c.SerializerTypes.Contains(this.SerializerType)).FirstOrDefault();
            if (jsonConverter != null)
            {
                contract.Converter = jsonConverter;
            }

            return contract;
        }
    }
}