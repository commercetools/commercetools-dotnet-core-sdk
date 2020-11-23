using System.Collections.Concurrent;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Validation;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class LocalizedStringConverter<T, S> : ICustomJsonMapper<T>
    {
        private readonly ICultureValidator cultureValidator;
        

        protected LocalizedStringConverter(ICultureValidator cultureValidator)
        {
            this.cultureValidator = cultureValidator;
        }

        public int Priority => 4;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                // Checks only the first key value pair; the assumption is all of them are of the same type.
                var firstValue = property.First as JProperty;
                var name = firstValue?.Name;
                if (name != null)
                {
                    return IsValid(name);
                }
            }
            return false;
        }

        private bool IsValid(string name)
        {
            return this.cultureValidator.IsCultureValid(name);
        }
    }
}
