using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public abstract class DecoratorTypeRetriever<T, S> : IDecoratorTypeRetriever<T> where S : TypeMarkerAttribute
    {
        private readonly IEnumerable<Type> derivedTypes;

        public DecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever)
        {
            this.derivedTypes = registeredTypeRetriever.GetRegisteredTypes<T>();
        }

        public Type GetTypeForToken(JToken token)
        {
            foreach (Type type in this.derivedTypes)
            {
                var attribute = type.CustomAttributes.Where(a => a.AttributeType == typeof(S)).FirstOrDefault();
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