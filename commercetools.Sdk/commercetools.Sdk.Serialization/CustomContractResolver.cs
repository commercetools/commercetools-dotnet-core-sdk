namespace commercetools.Sdk.Serialization
{
    using commercetools.Sdk.Domain;
    using Newtonsoft.Json.Serialization;
    using System;

    public class CustomContractResolver : DefaultContractResolver
    {
        private AttributeConverter attributeConverter;
        private MoneyConverter moneyConverter;

        public CustomContractResolver(AttributeConverter attributeConverter, MoneyConverter moneyConverter)
        {
            this.attributeConverter = attributeConverter;
            this.moneyConverter = moneyConverter;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            // This method will be run only once per type and the return type will be cached
            // https://www.newtonsoft.com/json/help/html/Performance.htm
            JsonContract contract = base.CreateContract(objectType);

            // If the number of custom converters becomes too big, register the converters elsewhere and loop through them instead
            if (objectType == typeof(Domain.Attribute))
            {
                contract.Converter = this.attributeConverter;
            }

            if (objectType == typeof(Money))
            {
                contract.Converter = this.moneyConverter;
            }

            return contract;
        }
    }
}