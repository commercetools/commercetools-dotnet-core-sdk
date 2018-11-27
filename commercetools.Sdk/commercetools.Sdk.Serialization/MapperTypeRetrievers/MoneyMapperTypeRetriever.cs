using commercetools.Sdk.Domain;
using System.Collections.Generic;

namespace commercetools.Sdk.Serialization
{
    public class MoneyMapperTypeRetriever : MapperTypeRetriever<Money>
    {
        public MoneyMapperTypeRetriever(IEnumerable<ICustomJsonMapper<Money>> customJsonMappers) : base(customJsonMappers)
        {
        }
    }
}