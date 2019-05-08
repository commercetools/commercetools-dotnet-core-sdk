using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    using Newtonsoft.Json;

    public class SerializerService : ISerializerService
    {
        private readonly JsonSerializerSettingsFactory jsonSerializerSettingsFactory;
        private readonly IModelValidator modelValidator;

        public SerializerService(JsonSerializerSettingsFactory jsonSerializerSettingsFactory, IModelValidator modelValidator)
        {
            this.jsonSerializerSettingsFactory = jsonSerializerSettingsFactory;
            this.modelValidator = modelValidator;
        }

        public T Deserialize<T>(string input)
        {
            var settings = this.jsonSerializerSettingsFactory.CreateDeserializationSettings(typeof(T));
            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        public string Serialize<T>(T input)
        {
            var settings = this.jsonSerializerSettingsFactory.CreateSerializationSettings(typeof(T));
            this.modelValidator.Validate(input);

            return JsonConvert.SerializeObject(input, settings);
        }
    }
}
