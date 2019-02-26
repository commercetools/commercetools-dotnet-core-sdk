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
        
        public CartFixture() : base()
        {
            this.CartToDelete = new List<Cart>();
            this.customerFixture = new CustomerFixture();
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
    }
}