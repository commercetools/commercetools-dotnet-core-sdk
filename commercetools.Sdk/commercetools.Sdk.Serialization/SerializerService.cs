namespace commercetools.Sdk.Serialization
{
    using Newtonsoft.Json;

    public class SerializerService : ISerializerService
    {
        private JsonSerializerSettingsFactory jsonSerializerSettingsFactory;

        public SerializerService(JsonSerializerSettingsFactory jsonSerializerSettingsFactory)
        {
            this.jsonSerializerSettingsFactory = jsonSerializerSettingsFactory;
        }

        public T Deserialize<T>(string input)
        {
            var settings = this.jsonSerializerSettingsFactory.CreateDeserilizationSettings(typeof(T));
            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        public string Serialize<T>(T input)
        {
            var settings = this.jsonSerializerSettingsFactory.CreateSerilizationSettings(typeof(T));
            return JsonConvert.SerializeObject(input, settings);
        }
    }
}