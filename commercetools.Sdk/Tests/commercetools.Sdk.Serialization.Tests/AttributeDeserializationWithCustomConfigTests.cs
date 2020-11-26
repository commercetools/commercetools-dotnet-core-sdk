using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class AttributeDeserializationWithCustomConfigTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public AttributeDeserializationWithCustomConfigTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

       
    }
}