using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Carts.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
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

    //    [Fact]
        public void CreateCart()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            CartDraft cartDraft = this.cartFixture.GetCartDraft();
            Cart cart = commerceToolsClient
                .ExecuteAsync(new CreateCommand<Cart>(cartDraft)).Result;
            this.cartFixture.CartToDelete.Add(cart);
            Assert.Equal(cartDraft.CustomerId, cart.CustomerId);
        }
      //  [Fact]
        public async void DeleteCartById()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            string cartId = "a62ceb19-84eb-4cda-a905-c506e44679e1";

            Cart cart = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<Cart>(new Guid(cartId))).Result;

            Cart deletedCart = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByIdCommand<Cart>(new Guid(cartId), cart.Version))
                .Result;
        }

        #region UpdateActions

     //   [Fact]
        public void UpdateCartAddLineItemByProductId()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            //Create Product, LineItemDraft and Cart
            Product product = this.cartFixture.CreateProduct();
            LineItemDraft lineItemDraft = this.cartFixture.GetLineItemDraft(product.Id,1, 5);
            Cart cart = this.cartFixture.CreateCart();


            AddLineItemByProductIdUpdateAction addLineItemUpdateAction = new AddLineItemByProductIdUpdateAction()
            {
                LineItem = lineItemDraft,
                ProductId = product.Id
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>>();
            updateActions.Add(addLineItemUpdateAction);

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.True(cart.LineItems.Count == lineItemDraft.Quantity);
        }
      //  [Fact]
        public void UpdateCartAddLineItemBySku()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            //Create Product, LineItemDraft and Cart
            Product product = this.cartFixture.CreateProduct();
            string sku = product.MasterData.Current.MasterVariant.Sku;
            LineItemDraft lineItemDraft = this.cartFixture.GetLineItemDraftBySku(sku, 5);
            Cart cart = this.cartFixture.CreateCart();


            AddLineItemBySkuUpdateAction addLineItemUpdateAction = new AddLineItemBySkuUpdateAction()
            {
                LineItem = lineItemDraft,
                Sku= sku,
                Quantity = lineItemDraft.Quantity
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>>();
            updateActions.Add(addLineItemUpdateAction);

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.True(cart.LineItems.Count == lineItemDraft.Quantity);
        }

        #endregion
    }
}
