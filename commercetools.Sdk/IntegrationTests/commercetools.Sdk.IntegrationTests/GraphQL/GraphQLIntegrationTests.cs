using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.GraphQL;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Categories.CategoriesFixture;

namespace commercetools.Sdk.IntegrationTests.GraphQL
{
    [Collection("Integration Tests")]
    public class GraphQLIntegrationTests
    {
        private readonly IClient client;

        public GraphQLIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateCustomerGroup()
        {
            await WithCategory(client, async category =>
            {
                var query = @"query Test($cid: String!) {
                    categories(where: $cid) {
                        results {
                            id
                        }
                    }
                }";
                var queryParameters = new GraphQLParameters(query, new Dictionary<string, object>
                {
                    {"cid", $"id = \"{category.Id}\""}
                });
                var graphQlCommand = new GraphQLCommand<dynamic>(queryParameters);
                var result = await client.ExecuteAsync(graphQlCommand);
                Assert.Equal(category.Id, result.data.categories.results[0].id.Value);
            });
        }
    }
}
