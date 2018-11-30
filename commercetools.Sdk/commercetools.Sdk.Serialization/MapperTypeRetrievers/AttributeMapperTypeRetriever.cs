using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class AttributeMapperTypeRetriever : SetMapperTypeRetriever<Domain.Attribute>
    {
        protected override Type SetType => typeof(Domain.Attributes.AttributeSet<>);

        public AttributeMapperTypeRetriever(IEnumerable<ICustomJsonMapper<Domain.Attribute>> customJsonMappers) : base(customJsonMappers)
        {
        }
    }
}