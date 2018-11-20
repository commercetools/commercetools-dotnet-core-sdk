using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class FieldMapperTypeRetriever : SetMapperTypeRetriever<Fields>
    {
        protected override Type SetType => typeof(Set<>);
        
        public FieldMapperTypeRetriever(IEnumerable<ICustomJsonMapper<Fields>> customJsonMappers) : base(customJsonMappers)
        {
        }
    }
}
