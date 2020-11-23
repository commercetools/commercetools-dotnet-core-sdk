using System.IO;
using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    using Newtonsoft.Json;

    public class SerializerService : ISerializerService
    {
        private readonly JsonSerializerSettings serializerSettings;
        private readonly JsonSerializerSettings deserializerSettings;
        private readonly IModelValidator modelValidator;

        public SerializerService(JsonSerializerSettingsFactory jsonSerializerSettingsFactory, IModelValidator modelValidator)
        {
            this.modelValidator = modelValidator;
            this.serializerSettings = jsonSerializerSettingsFactory.CreateSerializationSettings();
            this.deserializerSettings = jsonSerializerSettingsFactory.CreateDeserializationSettings();
            
        }

        public T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, deserializerSettings);
        }

        public T Deserialize<T>(Stream input)
        {
            T result;
            using (var sr = new StreamReader(input))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var serializer = JsonSerializer.CreateDefault(deserializerSettings);

                result = serializer.Deserialize<T>(reader);
            }

            return result;
        }

        
        public string Serialize<T>(T input)
        {
            this.modelValidator.Validate(input);

            return JsonConvert.SerializeObject(input, serializerSettings);
        }
    }
}
