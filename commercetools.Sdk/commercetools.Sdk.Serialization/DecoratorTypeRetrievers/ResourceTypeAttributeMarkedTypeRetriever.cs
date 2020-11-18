using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Registration;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    /// <summary>
    /// Return Types which marked with custom ResourceType Attribute
    /// </summary>
    internal class ResourceTypeAttributeMarkedTypeRetriever : IDecoratorTypeRetriever<ResourceTypeAttribute>
    {
        private readonly IEnumerable<Type> markedTypes;
        private ConcurrentDictionary<string, Type> propertyTypes = new ConcurrentDictionary<string, Type>();
        public ResourceTypeAttributeMarkedTypeRetriever(ITypeRetriever typeRetriever)
        {
            this.markedTypes = typeof(ResourceTypeAttribute).GetMarkedTypes();
        }

        /// <summary>
        /// Pass ReferenceTypeId as jsonToken and return type that has custom ResourceType Attribute with this referenceTypeId
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Type GetTypeForToken(JToken token)
        {
            return propertyTypes.GetOrAdd(token.Value<string>(), (typeId) => {
                var referenceTypeId = (ReferenceTypeId)typeId.GetEnum(typeof(ReferenceTypeId));
                        
                foreach (Type type in this.markedTypes)
                {
                    var attribute = type.GetCustomAttributes(typeof(ResourceTypeAttribute)).FirstOrDefault();
                    if (attribute is ResourceTypeAttribute typeAttribute && typeAttribute.Value.Equals(referenceTypeId))
                    {
                        return type;
                    }
                }
    
                return null;
            });
            
        }
    }
}
