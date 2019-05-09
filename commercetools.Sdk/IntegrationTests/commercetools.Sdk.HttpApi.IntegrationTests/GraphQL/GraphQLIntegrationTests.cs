using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.HttpApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain.Query;
using Xunit;
using commercetools.Sdk.Domain.Categories.UpdateActions;
using commercetools.Sdk.Domain.GraphQL;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using ChangeNameUpdateAction = commercetools.Sdk.Domain.Categories.UpdateActions.ChangeNameUpdateAction;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    // All integration tests need to have the same collection name.
    [Collection("Integration Tests")]
    public class GraphQLIntegrationTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly GraphQLFixture graphqlFixture;

        public GraphQLIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.graphqlFixture = new GraphQLFixture(serviceProviderFixture);
        }

        [Fact]
        public void GetCategoryById()
        {
            IClient commerceToolsClient = this.graphqlFixture.GetService<IClient>();
            var category = this.graphqlFixture.CreateCategory();


            var query = new GraphQLParameters(
                @"query Sphere($cid: String!) {
                    categories(where: $cid) {
                        results {
                            id
                        }
                    }
                }",
                new Dictionary<string, object>()
                {
                    {"cid", $"id = \"{category.Id}\""}
                }
            );
            var graphQlCommand = new GraphQLCommand<dynamic>(query);

            var result = commerceToolsClient.ExecuteAsync(graphQlCommand).Result;
            Assert.Equal(category.Id, result.data.categories.results[0].id.Value);
        }
    }
}
