using System.Collections.Concurrent;
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
        private readonly ConcurrentDictionary<string, Type> cachedTypes = new ConcurrentDictionary<string, Type>();

        public DecoratorTypeRetriever(ITypeRetriever typeRetriever)
        {
            this.derivedTypes = typeRetriever.GetTypes<T>();
        }

        public Type GetTypeForToken(JToken token)
        {
            return cachedTypes.GetOrAdd(token.Value<string>(), tokenValue => {
                foreach (var type in this.derivedTypes)
                {
                    var attribute = type.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(TypeMarkerAttribute));
                    if (attribute == null) continue;
                    if (attribute.ConstructorArguments[0].Value.ToString() == tokenValue)
                    {
                        return type;
                    }
                }
                return null;            
            });
        }
    }
}