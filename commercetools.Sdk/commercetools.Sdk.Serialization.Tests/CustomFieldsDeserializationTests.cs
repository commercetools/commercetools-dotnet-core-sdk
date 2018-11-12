using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class CustomFieldsDeserializationTests
    {
        [Fact]
        public void CustomFieldsString()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/CustomFields/AllCustomFieldTypes.json");
            Category deserialized = serializerService.Deserialize<Category>(serialized);
            Assert.IsType<string>(deserialized.Custom.Fields["string"]);
        }
    }
}
