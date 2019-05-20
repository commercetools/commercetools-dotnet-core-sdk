using Newtonsoft.Json;
using System.Collections.Generic;

namespace commercetools.Sdk.Serialization
{
    public abstract class JsonConverterBase : JsonConverter
    {
        public abstract List<SerializerType> SerializerTypes { get; }
    }
}
