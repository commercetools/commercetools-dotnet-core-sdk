using System;
using Newtonsoft.Json;

namespace commercetools.Sdk.Serialization.JsonConverters
{
    public interface IFieldConverterReader
    {

        object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);
    }
    
}