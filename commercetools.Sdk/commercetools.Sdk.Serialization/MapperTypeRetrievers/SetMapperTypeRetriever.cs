using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class SetMapperTypeRetriever<T> : MapperTypeRetriever<T>
    {
        protected abstract Type SetType { get; }

        public SetMapperTypeRetriever(IEnumerable<ICustomJsonMapper<T>> customJsonMappers) : base(customJsonMappers)
        {
        }

        public override Type GetTypeForToken(JToken token)
        {
            Type type;
            if (IsSetAttribute(token))
            {
                Type firstAttributeType = base.GetTypeForToken(token[0]);
                type = this.SetType.MakeGenericType(firstAttributeType);
            }
            else
            {
                type = base.GetTypeForToken(token);
            }
            return type;
        }

        private bool IsSetAttribute(JToken valueProperty)
        {
            if (valueProperty != null)
            {
                return valueProperty.HasValues && valueProperty.Type == JTokenType.Array;
            }
            return false;
        }
    }
}