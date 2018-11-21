using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public class MoneyMapperTypeRetriever : MapperTypeRetriever<Money>
    {
        public MoneyMapperTypeRetriever(IEnumerable<ICustomJsonMapper<Money>> customJsonMappers) : base(customJsonMappers)
        {

        }
    }
}
