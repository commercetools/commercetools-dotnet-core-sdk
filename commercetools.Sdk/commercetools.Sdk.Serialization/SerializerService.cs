namespace commercetools.Sdk.Serialization
{
    using Newtonsoft.Json;
    using System;

    public class SerializerService : ISerializerService
    {
        private Func<Type, JsonSerializerSettings> settingsFactory;

        public SerializerService(Func<Type, JsonSerializerSettings> settingsFactory)
        {
            this.settingsFactory = settingsFactory;
        }

        public T Deserialize<T>(string input)
        {
            var settings = this.settingsFactory(typeof(T));

            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        public string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}