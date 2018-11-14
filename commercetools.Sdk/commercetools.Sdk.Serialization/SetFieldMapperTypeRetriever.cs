using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class SetFieldMapperTypeRetriever : SetMapperTypeRetriever<Fields>
    {
        protected override Type SetType => typeof(Set<>);
        
        public SetFieldMapperTypeRetriever(IEnumerable<ICustomJsonMapper<Fields>> customJsonMappers) : base(customJsonMappers)
        {
        }
    }
}
