using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Registration;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class DecoratorTypeRetriever<T> : IDecoratorTypeRetriever<T>
    {
        private readonly IEnumerable<Type> derivedTypes;

        public DecoratorTypeRetriever(ITypeRetriever typeRetriever)
        {
            this.derivedTypes = typeRetriever.GetTypes<T>();
        }

        public Type GetTypeForToken(JToken token)
        {
            foreach (Type type in this.derivedTypes)
            {
                var attribute = type.CustomAttributes.Where(a => a.AttributeType == typeof(TypeMarkerAttribute)).FirstOrDefault();
                if (attribute != null)
                {
                    if (attribute.ConstructorArguments[0].Value.ToString() == token.Value<string>())
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}