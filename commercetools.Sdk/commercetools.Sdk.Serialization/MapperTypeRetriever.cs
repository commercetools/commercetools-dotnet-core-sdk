using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public class MapperTypeRetriever<T> : IMapperTypeRetriever<T>
    {
        private readonly IEnumerable<ICustomJsonMapper<T>> customJsonMappers;

        public MapperTypeRetriever(IEnumerable<ICustomJsonMapper<T>> customJsonMappers)
        {
            this.customJsonMappers = customJsonMappers;
        }    

        public Type GetTypeForToken(JToken token)
        {
            foreach (var customJsonMapper in this.customJsonMappers.OrderBy(c => c.Priority))
            {
                if (customJsonMapper.CanConvert(token))
                {
                    return customJsonMapper.Type;
                }
            }
            return null;
        }
    }
}
