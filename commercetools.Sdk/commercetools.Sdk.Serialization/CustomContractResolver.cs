namespace commercetools.Sdk.Serialization
{
    using Newtonsoft.Json.Serialization;
    using System;

    public class CustomContractResolver : DefaultContractResolver
    {
        public static readonly CustomContractResolver Instance = new CustomContractResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            // This method will be run only once per type and the return type will be cached
            // https://www.newtonsoft.com/json/help/html/Performance.htm
            JsonContract contract = base.CreateContract(objectType);

            if (objectType == typeof(Domain.Attribute))
            {
                contract.Converter = new AttributeConverter();
            }

            return contract;
        }
    }
}