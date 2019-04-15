using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.HttpApi.IntegrationTests.Customers;
using commercetools.Sdk.HttpApi.IntegrationTests.Products;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ShoppingLists
{
    public class ShoppingListFixture : ClientFixture, IDisposable
    {
        public List<ShoppingList> ShoppingListToDelete { get; private set; }

        private readonly CustomerFixture customerFixture;
        private readonly ProductFixture productFixture;

        public ShoppingListFixture() : base()
        {
            this.ShoppingListToDelete = new List<ShoppingList>();
            this.customerFixture = new CustomerFixture();
            this.productFixture = new ProductFixture();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ShoppingListToDelete.Reverse();
            foreach (ShoppingList shoppingList in this.ShoppingListToDelete)
            {
                ShoppingList deletedShoppingList = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<ShoppingList>(new Guid(shoppingList.Id), shoppingList.Version)).Result;
            }
            this.customerFixture.Dispose();
            this.productFixture.Dispose();
        }

        /// <summary>
        /// Get ShoppingList Draft
        /// </summary>
        /// <returns></returns>

        public ShoppingListDraft GetShoppingListDraft(bool withCustomer = true, bool withLineItem = false)
        {
            string name = $"ShoppingList_{this.RandomInt()}";
            ShoppingListDraft shoppingListDraft = new ShoppingListDraft
            {
                Key = this.RandomString(10),
                Slug = new LocalizedString(){{"en", name}},
                Name = new LocalizedString(){{"en", name}},
                DeleteDaysAfterLastModification = 30
            };
            if (withCustomer)
            {
                Customer customer = this.customerFixture.CreateCustomer();
                this.customerFixture.CustomersToDelete.Add(customer);
                shoppingListDraft.Customer = new ResourceIdentifier<Customer> {Id = customer.Id};
            }
            if (withLineItem)
            {
                Product product = this.CreateProduct();
                LineItemDraft lineItemDraft =
                    this.GetLineItemDraftBySku(product.MasterData.Current.MasterVariant.Sku, 2);
                shoppingListDraft.LineItems = new List<LineItemDraft>() {lineItemDraft};
            }

            return shoppingListDraft;
        }

        public ShoppingList CreateShoppingList(bool withCustomer = true, bool withLineItem = false)
        {
            return this.CreateShoppingList(this.GetShoppingListDraft(withCustomer, withLineItem));
        }

        public ShoppingList CreateShoppingList(ShoppingListDraft shoppingListDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            ShoppingList shoppingList = commerceToolsClient.ExecuteAsync(new CreateCommand<ShoppingList>(shoppingListDraft)).Result;
            return shoppingList;
        }

        private Product CreateProduct()
        {
            Product product = this.productFixture.CreateProduct(withVariants:true, publish: true);
            this.productFixture.ProductsToDelete.Add(product);
            return product;
        }

        public LineItemDraft GetLineItemDraftBySku(string sku, int quantity = 1)
        {
            LineItemDraft lineItemDraft = new LineItemDraft();
            lineItemDraft.Sku = sku;
            lineItemDraft.Quantity = quantity;
            return lineItemDraft;
        }
    }
}
