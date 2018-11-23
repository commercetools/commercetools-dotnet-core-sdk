using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TypeIntegrationTests : IClassFixture<TypeFixture>
    {
        private readonly TypeFixture typeFixture;

        public TypeIntegrationTests(TypeFixture typeFixture)
        {
            this.typeFixture = typeFixture;
        }

        [Fact]
        public void CreateType()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            TypeDraft typeDraft = this.typeFixture.CreateTypeDraft();
            Type createdType = commerceToolsClient.ExecuteAsync(new CreateCommand<Type>(typeDraft)).Result;
        }
    }
}
