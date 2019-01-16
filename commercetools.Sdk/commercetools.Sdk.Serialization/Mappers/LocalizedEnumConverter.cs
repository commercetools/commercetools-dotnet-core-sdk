using commercetools.Sdk.Domain.Validation;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class LocalizedEnumConverter<T, S> : ICustomJsonMapper<T>
    {
        private readonly ICultureValidator cultureValidator;

        protected LocalizedEnumConverter(ICultureValidator cultureValidator)
        {
            this.cultureValidator = cultureValidator;
        }

        public int Priority => 2;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                var keyProperty = property["key"];
                if (keyProperty != null)
                {
                    var labelProperty = property["label"];
                    if (labelProperty != null && labelProperty.HasValues)
                    {
                        // Checks only the first key value pair; the assumption is all of them are of the same type
                        var firstValue = labelProperty.First as JProperty;
                        var name = firstValue?.Name;
                        if (name != null && this.cultureValidator.IsCultureValid(name))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}