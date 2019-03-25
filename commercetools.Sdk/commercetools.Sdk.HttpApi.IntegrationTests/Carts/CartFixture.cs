using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.HttpApi.IntegrationTests.CustomerGroups;
using commercetools.Sdk.HttpApi.IntegrationTests.Customers;
using commercetools.Sdk.HttpApi.IntegrationTests.DiscountCodes;
using commercetools.Sdk.HttpApi.IntegrationTests.ShippingMethods;
using commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Carts
{
    public class CartFixture : ClientFixture, IDisposable
    {
        public List<Cart> CartToDelete { get; }

        private readonly CustomerFixture customerFixture;
        private readonly ProductFixture productFixture;
        private readonly ShippingMethodsFixture shippingMethodsFixture;
        private readonly TaxCategoryFixture taxCategoryFixture;
        private readonly DiscountCodeFixture discountCodeFixture;
        private readonly CustomerGroupFixture customerGroupFixture;
        private readonly TypeFixture typeFixture;

        public CartFixture() : base()
        {
            this.CartToDelete = new List<Cart>();
            this.customerFixture = new CustomerFixture();
            this.productFixture = new ProductFixture();
            this.shippingMethodsFixture = new ShippingMethodsFixture();
            this.taxCategoryFixture = new TaxCategoryFixture();
            this.discountCodeFixture = new DiscountCodeFixture();
            this.customerGroupFixture = new CustomerGroupFixture();
            this.typeFixture = new TypeFixture();
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
            this.shippingMethodsFixture.Dispose();
            this.taxCategoryFixture.Dispose();
            this.discountCodeFixture.Dispose();
            this.customerGroupFixture.Dispose();
            this.typeFixture.Dispose();
        }

        public CartDraft GetCartDraft(bool withCustomer = true, bool withDefaultShippingCountry = true)
        {
            var address = withDefaultShippingCountry
                ? new Address() {Country = "DE"}
                : new Address() {Country = this.GetRandomEuropeCountry()};

            CartDraft cartDraft = new CartDraft();
            cartDraft.Currency = "EUR";
            cartDraft.ShippingAddress = address;
            cartDraft.DeleteDaysAfterLastModification = 30;

            if (withCustomer)//then create customer and attach it to the cart
            {
                Customer customer = this.customerFixture.CreateCustomer();
                this.customerFixture.CustomersToDelete.Add(customer);
                cartDraft.CustomerId = customer.Id;
            }
            return cartDraft;
        }

        public Cart CreateCart(bool withCustomer = true, bool withDefaultShippingCountry = true)
        {
            return this.CreateCart(this.GetCartDraft(withCustomer, withDefaultShippingCountry));
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
            Product product = this.productFixture.CreateProduct(withVariants:true, publish:true);
            this.productFixture.ProductsToDelete.Add(product);
            return product;
        }

        public ShippingMethod CreateShippingMethod(string shippingCountry = null)
        {
            ShippingMethod shippingMethod = this.shippingMethodsFixture.CreateShippingMethod(shippingCountry);
            this.shippingMethodsFixture.ShippingMethodsToDelete.Add(shippingMethod);
            return shippingMethod;
        }

        public TaxCategory CreateNewTaxCategory()
        {
            TaxCategory taxCategory = this.taxCategoryFixture.CreateTaxCategory();
            this.taxCategoryFixture.TaxCategoriesToDelete.Add(taxCategory);
            return taxCategory;
        }

        public ShippingRateDraft GetShippingRateDraft()
        {
            ShippingRateDraft rate = new ShippingRateDraft()
            {
                Price = Money.Parse("1.00 EUR"),
                FreeAbove = Money.Parse("100.00 EUR")
            };
            return rate;
        }

        public DiscountCode CreateDiscountCode(string code)
        {
            DiscountCode discountCode = this.discountCodeFixture.CreateDiscountCode(code);
            this.discountCodeFixture.DiscountCodesToDelete.Add(discountCode);
            return discountCode;
        }

        public CustomerGroup CreateCustomerGroup()
        {
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            this.customerGroupFixture.CustomerGroupsToDelete.Add(customerGroup);
            return customerGroup;
        }

        public Type CreateCustomType()
        {
            Type customType = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(customType);
            return customType;
        }
        public Fields CreateNewFields()
        {
            Fields fields = this.typeFixture.CreateNewFields();
            return fields;
        }

        public ExternalTaxAmountDraft GetExternalTaxAmountDraft()
        {
            var externalTaxAmount = new ExternalTaxAmountDraft()
            {
                TotalGross = Money.Parse("100 EUR"),
                TaxRate = this.GetExternalTaxRateDraft()
            };
            return externalTaxAmount;
        }

        public ExternalTaxRateDraft GetExternalTaxRateDraft()
        {
            var externalTaxRateDraft = new ExternalTaxRateDraft
            {
                Amount = this.RandomDouble(),
                Name = "Test tax",
                Country = "DE"

            };
            return externalTaxRateDraft;
        }

        public Address GetRandomAddress()
        {
            var shippingAddress = new Address()
            {
                Country = "DE",
                PostalCode = this.RandomInt().ToString(),
                StreetName = this.RandomString(10)
            };
            return shippingAddress;
        }


    }
}
