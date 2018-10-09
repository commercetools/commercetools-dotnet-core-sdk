namespace commercetools.Sdk.Serialization
{
    using commercetools.Sdk.Domain;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly IEnumerable<JsonConverter> registeredConverters;

        public CustomContractResolver(IEnumerable<JsonConverter> registeredConverters)
        {
            this.registeredConverters = registeredConverters;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            // This method will be run only once per type and the return type will be cached
            // https://www.newtonsoft.com/json/help/html/Performance.htm
            JsonContract contract = base.CreateContract(objectType);

            JsonConverter jsonConverter = registeredConverters.Where(c => c.CanConvert(objectType)).FirstOrDefault();
            if (jsonConverter != null)
            {
                contract.Converter = jsonConverter;
            }            

            return contract;
        }
    }
}