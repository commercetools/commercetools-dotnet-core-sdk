using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.GraphQL;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.HttpApi.CommandBuilders;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Categories.CategoriesFixture;
using static commercetools.Sdk.IntegrationTests.Carts.CartsFixture;
using static commercetools.Sdk.IntegrationTests.Orders.OrdersFixture;

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

        [Fact]
        public async Task UpdateAnOrder()
        {
            //Arrange
            var orderNumber = $"order-{TestingUtility.RandomInt()}";
            var query = @"mutation ($orderId:String, $orderNumber:String){
                                updateOrder(
                                             version: 1,
                                             id:$orderId,
                                             actions: [
     	                                                {
                                                            changeOrderState:{
                                                              orderState: Confirmed
                                                            }
                                                        },
                                                        {
                                                            setOrderNumber:{
                                                              orderNumber: $orderNumber
                                                            }
                                                        }
                                                      ]
                                            ) {
                                                id
                                              } 
                                    }";
            
            //act
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    
                    //create order with null orderNumber
                    await WithOrder(client, 
                        draft => DefaultOrderFromCartDraftWithNumber(draft, cart, orderNumber: null),
                        async order =>
                        {
                            Assert.NotNull(order);
                            Assert.Null(order.OrderNumber);
                            
                            var queryParameters = new GraphQLParameters(query, new Dictionary<string, object>
                            {
                                {"orderId", order.Id},
                                {"orderNumber", orderNumber}
                            });
                            var graphQlCommand = new GraphQLCommand<dynamic>(queryParameters);
                            var result = await client.ExecuteAsync(graphQlCommand);
                            Assert.NotNull(result);

                            //retrieve updated order
                            var updatedOrder = await client.Builder().Orders().GetById(order.Id).ExecuteAsync();

                            //assert
                            Assert.NotNull(updatedOrder);
                            Assert.Equal(OrderState.Confirmed, updatedOrder.OrderState);
                            Assert.Equal(orderNumber, updatedOrder.OrderNumber);
                        });
                });
        }
    }
}