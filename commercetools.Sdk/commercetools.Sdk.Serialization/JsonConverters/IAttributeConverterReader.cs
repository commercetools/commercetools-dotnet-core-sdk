using System;
using Newtonsoft.Json;

namespace commercetools.Sdk.Serialization.JsonConverters
{
    public interface IAttributeConverterReader
    {

        object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);
    }
    
}