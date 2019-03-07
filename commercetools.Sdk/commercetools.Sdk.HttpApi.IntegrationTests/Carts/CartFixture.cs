using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.HttpApi.IntegrationTests.Customers;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Carts
{
    public class CartFixture : ClientFixture, IDisposable
    {
        public List<Cart> CartToDelete { get; }

        private CustomerFixture customerFixture;
        private ProductFixture productFixture;
        
        public CartFixture() : base()
        {
            this.CartToDelete = new List<Cart>();
            this.customerFixture = new CustomerFixture();
            this.productFixture = new ProductFixture();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.CartToDelete.Reverse();
            foreach (Cart cart in this.CartToDelete)
            {
                Cart deletedType = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<Cart>(new Guid(cart.Id),
                        cart.Version)).Result;
            }
            this.customerFixture.Dispose();
            this.productFixture.Dispose();
        }
        
        public CartDraft GetCartDraft()
        {
            Customer customer = this.customerFixture.CreateCustomer();
            this.customerFixture.CustomersToDelete.Add(customer);
            
            CartDraft cartDraft = new CartDraft();
            cartDraft.CustomerId = customer.Id;
            cartDraft.Currency = "EUR";
            cartDraft.ShippingAddress = new Address()
            {
                Country = "DE"
            };
            cartDraft.DeleteDaysAfterLastModification = 30;
            return cartDraft;
        }
        
        public Cart CreateCart()
        {
            return this.CreateCart(this.GetCartDraft());
        }

        public Cart CreateCart(CartDraft cartDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Cart cart = commerceToolsClient.ExecuteAsync(new CreateCommand<Cart>(cartDraft)).Result;
            return cart;
        }

        /// <summary>
        /// Get Line Item Draft, by default master variant is selected
        /// </summary>
        /// <param name="productId">product Id</param>
        /// <param name="variantId">variant Id - by default master variant Id</param>
        /// <param name="quantity">quantity of this product variant</param>
        /// <returns>line item draft</returns>
        public LineItemDraft GetLineItemDraft(string productId, int variantId = 1, int quantity = 1)
        {
            LineItemDraft lineItemDraft = new LineItemDraft();
            lineItemDraft.ProductId = productId;
            lineItemDraft.VariantId = variantId.ToString();
            lineItemDraft.Quantity = quantity;
            return lineItemDraft;
        }
        public LineItemDraft GetLineItemDraftBySku(string sku, int quantity = 1)
        {
            LineItemDraft lineItemDraft = new LineItemDraft();
            lineItemDraft.Sku = sku;
            lineItemDraft.Quantity = quantity;
            return lineItemDraft;
        }

        public Product CreateProduct()
        {
            Product product = this.productFixture.CreateProductAndPublishIt(true);
            this.productFixture.ProductsToDelete.Add(product);
            return product;
        }
        
    }
}