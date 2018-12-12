using System.Collections.Generic;
using commercetools.Sdk.Domain.Products.Attributes;
using Type = System.Type;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization
{
    public class AttributeMapperTypeRetriever : SetMapperTypeRetriever<Attribute>
    {
        protected override Type SetType => typeof(AttributeSet<>);

        public AttributeMapperTypeRetriever(IEnumerable<ICustomJsonMapper<Attribute>> customJsonMappers) : base(customJsonMappers)
        {
        }
    }
}