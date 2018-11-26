using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public abstract class JsonConverterBase : JsonConverter
    {
        public abstract List<SerializerType> SerializerTypes { get; }
    }
}
