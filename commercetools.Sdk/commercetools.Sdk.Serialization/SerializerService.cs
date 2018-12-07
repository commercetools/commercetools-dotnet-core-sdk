namespace commercetools.Sdk.Serialization
{
    using System.Linq;
    using Domain.Validation;
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
            var settings = this.jsonSerializerSettingsFactory.CreateDeserilizationSettings(typeof(T));
            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        public string Serialize<T>(T input)
        {
            var settings = this.jsonSerializerSettingsFactory.CreateSerilizationSettings(typeof(T));
            var results = this.modelValidator.IsValid(input).ToList();
            if (results.Count != 0)
            {
                ValidationException exception = new ValidationException()
                {
                    ValidationResults = results
                };
                throw exception;
            }

            return JsonConvert.SerializeObject(input, settings);
        }
    }
}