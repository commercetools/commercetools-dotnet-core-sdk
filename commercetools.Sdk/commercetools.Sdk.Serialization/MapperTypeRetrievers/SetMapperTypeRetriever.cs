﻿using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Products.Attributes;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class SetMapperTypeRetriever<T> : MapperTypeRetriever<T>
    {
        private readonly ConcurrentDictionary<Type, Type> attributeTypes = new ConcurrentDictionary<Type, Type>();
        
        protected abstract Type SetType { get; }

        public SetMapperTypeRetriever(IEnumerable<ICustomJsonMapper<T>> customJsonMappers) : base(customJsonMappers)
        {
        }

        public override Type GetTypeForToken(JToken token)
        {
            Type type;
            if (token.IsSetAttribute())
            {
                if (token.HasValues)
                {
                    Type firstAttributeType = base.GetTypeForToken(token[0]);
                    type = attributeTypes.GetOrAdd(firstAttributeType, this.SetType.MakeGenericType(firstAttributeType));
                }
                else
                {
                    // If the array contains no elements
                    type = attributeTypes.GetOrAdd(typeof(object), this.SetType.MakeGenericType(typeof(object)));
                }
            }
            else
            {
                type = base.GetTypeForToken(token);
            }
            return type;
        }
    }
}