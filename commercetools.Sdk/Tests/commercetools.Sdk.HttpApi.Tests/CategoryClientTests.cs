using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Serialization;
using commercetools.Sdk.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryClientTests
    {
        [Fact(Skip = "Not implemented fully yet")]
        public void GetCategoryById()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            IClient commerceToolsClient = new Client(null, null, serializerService);
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd6";
            Category category = commerceToolsClient.Execute(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }
    }
}
