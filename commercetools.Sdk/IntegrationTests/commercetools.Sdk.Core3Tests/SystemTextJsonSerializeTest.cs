namespace commercetools.Sdk.Core3Tests
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.Domain.Projects;
    using Xunit;
    using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
    using Type = System.Type;

    [Collection("Integration Tests")]
    public class SystemTextJsonSerializeTest
    {
        private readonly IClient client;

        public SystemTextJsonSerializeTest(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task SystemTextJsonSerializeProject()
        {
            var project = await client.ExecuteAsync(new GetProjectCommand());
            Assert.IsType<Project>(project);

            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.IgnoreNullValues = true;
            var projectJson = JsonSerializer.Serialize(project, options);
            Assert.NotEmpty(projectJson);
        }

        [Fact]
        public async Task SystemTextJsonSerializeProduct()
        {
            var products = await client.ExecuteAsync(new QueryCommand<Product>());
            Assert.IsType<PagedQueryResult<Product>>(products);

            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.IgnoreNullValues = true;
            options.Converters.Add(new ReferenceConverterFactory());
            options.Converters.Add(new AttributeConverter());

            var productsJson = JsonSerializer.Serialize(products, options);
            Assert.NotEmpty(productsJson);
        }

    }

    class ReferenceConverter<T> : JsonConverter<Reference<T>>
    {
        public override Reference<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Reference<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("typeId", value.GetResourceType().GetDescription());
            writer.WriteString("id", value.Id);
            writer.WriteEndObject();
        }
    }

    class ReferenceConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            var type = typeToConvert;

            if (!type.IsGenericTypeDefinition)
                type = type.GetGenericTypeDefinition();

            return type == typeof(Reference<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert,
            JsonSerializerOptions options)
        {
            var keyType = typeToConvert.GenericTypeArguments[0];
            var converterType = typeof(ReferenceConverter<>).MakeGenericType(keyType);

            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }

    class AttributeConverter : JsonConverter<Attribute>
    {
        public override Attribute Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Attribute value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("name", value.Name);
            writer.WriteEndObject();
        }
    }
}
