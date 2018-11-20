using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Attributes;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class AttributeMapperTypeRetriever : SetMapperTypeRetriever<Domain.Attribute>
    {
        protected override Type SetType => typeof(Domain.Attributes.Set<>);
        
        public AttributeMapperTypeRetriever(IEnumerable<ICustomJsonMapper<Domain.Attribute>> customJsonMappers) : base(customJsonMappers)
        {
        }
    }
}
