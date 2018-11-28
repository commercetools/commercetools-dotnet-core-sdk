using commercetools.Sdk.Domain;
using System.Collections.Generic;

namespace commercetools.Sdk.Serialization
{
    public class MoneyMapperTypeRetriever : MapperTypeRetriever<BaseMoney>
    {
        public MoneyMapperTypeRetriever(IEnumerable<ICustomJsonMapper<BaseMoney>> customJsonMappers) : base(customJsonMappers)
        {
        }
    }
}