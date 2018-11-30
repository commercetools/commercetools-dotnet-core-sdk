using commercetools.Sdk.Domain;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class FieldMapperTypeRetriever : SetMapperTypeRetriever<Fields>
    {
        protected override Type SetType => typeof(FieldSet<>);

        public FieldMapperTypeRetriever(IEnumerable<ICustomJsonMapper<Fields>> customJsonMappers) : base(customJsonMappers)
        {
        }
    }
}