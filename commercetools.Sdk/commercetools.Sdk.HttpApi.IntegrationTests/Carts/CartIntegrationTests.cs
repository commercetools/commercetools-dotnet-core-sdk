using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Carts;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Carts
{
    [Collection("Integration Tests")]
    public class CartIntegrationTests : IClassFixture<CartFixture>
    {
        private readonly CartFixture cartFixture;

        public CartIntegrationTests(CartFixture cartFixture)
        {
            this.cartFixture = cartFixture;
        }
        
        [Fact]
        public void CreateCart()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            CartDraft cartDraft = this.cartFixture.GetCartDraft();
            Cart cart = commerceToolsClient
                .ExecuteAsync(new CreateCommand<Cart>(cartDraft)).Result;
            this.cartFixture.CartToDelete.Add(cart);
            Assert.Equal(cartDraft.CustomerId, cart.CustomerId);
        }
    }
}