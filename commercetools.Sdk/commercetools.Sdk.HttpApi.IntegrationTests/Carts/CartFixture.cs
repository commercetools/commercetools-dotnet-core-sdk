using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.HttpApi.IntegrationTests.CustomerGroups;
using commercetools.Sdk.HttpApi.IntegrationTests.Customers;
using commercetools.Sdk.HttpApi.IntegrationTests.DiscountCodes;
using commercetools.Sdk.HttpApi.IntegrationTests.Payments;
using commercetools.Sdk.HttpApi.IntegrationTests.Products;
using commercetools.Sdk.HttpApi.IntegrationTests.Project;
using commercetools.Sdk.HttpApi.IntegrationTests.ShippingMethods;
using commercetools.Sdk.HttpApi.IntegrationTests.ShoppingLists;
using commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories;
using LineItemDraft = commercetools.Sdk.Domain.Carts.LineItemDraft;
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
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly PaymentsFixture paymentsFixture;
        private readonly ProjectFixture projectFixture;

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
            this.shoppingListFixture = new ShoppingListFixture();
            this.paymentsFixture = new PaymentsFixture();
            this.projectFixture = new ProjectFixture();
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
            this.shoppingListFixture.Dispose();
        }

        public CartDraft GetCartDraft(bool withCustomer = true, bool withDefaultShippingCountry = true, bool withItemShippingAddress = false)
        {
            string country = withDefaultShippingCountry ? "DE" : this.GetRandomEuropeCountry();

            var address = new Address { Country = country, Key = this.RandomString(10)};

            CartDraft cartDraft = new CartDraft();
            cartDraft.Currency = "EUR";
            cartDraft.ShippingAddress = address;
            cartDraft.DeleteDaysAfterLastModification = 1;

            if (withItemShippingAddress)
            {
                cartDraft.ItemShippingAddresses = new List<Address> {address};
            }
            if (withCustomer)//then create customer and attach it to the cart
            {
                Customer customer = this.customerFixture.CreateCustomer();
                this.customerFixture.CustomersToDelete.Add(customer);
                cartDraft.CustomerId = customer.Id;
            }
            return cartDraft;
        }

        public Cart CreateCart(TaxMode taxMode = TaxMode.Platform,bool withCustomer = true, bool withDefaultShippingCountry = true,  bool withItemShippingAddress = false)
        {
            CartDraft cartDraft = this.GetCartDraft(withCustomer, withDefaultShippingCountry, withItemShippingAddress);
            cartDraft.TaxMode = taxMode;
            return this.CreateCart(cartDraft);
        }

        public Cart CreateCartWithLineItem(TaxMode taxMode = TaxMode.Platform,bool withCustomer = true, bool withDefaultShippingCountry = true,  bool withItemShippingAddress = false)
        {
            Product product = this.CreateProduct();
            LineItemDraft lineItemDraft = this.GetLineItemDraftBySku(product.MasterData.Current.MasterVariant.Sku, quantity: 6);
            CartDraft cartDraft = this.GetCartDraft(withCustomer, withDefaultShippingCountry, withItemShippingAddress);
            cartDraft.LineItems = new List<LineItemDraft>{ lineItemDraft };
            cartDraft.TaxMode = taxMode;
            Cart cart = this.CreateCart(cartDraft);
            return cart;
        }

        public Cart CreateCartWithCustomLineItem(bool withCustomer = true, bool withDefaultShippingCountry = true,  bool withItemShippingAddress = false)
        {
            var customLineItemDraft = this.GetCustomLineItemDraft();
            CartDraft cartDraft = this.GetCartDraft(withCustomer, withDefaultShippingCountry, withItemShippingAddress);
            cartDraft.CustomLineItems = new List<CustomLineItemDraft>{ customLineItemDraft };
            Cart cart = this.CreateCart(cartDraft);
            return cart;
        }

        public Cart CreateCartWithCustomLineItemWithSpecificTaxMode(TaxMode taxMode,bool withCustomer = true, bool withDefaultShippingCountry = true,  bool withItemShippingAddress = false)
        {
            var customLineItemDraft = this.GetCustomLineItemDraft();
            CartDraft cartDraft = this.GetCartDraft(withCustomer, withDefaultShippingCountry, withItemShippingAddress);
            cartDraft.CustomLineItems = new List<CustomLineItemDraft>{ customLineItemDraft };
            cartDraft.TaxMode = taxMode;
            Cart cart = this.CreateCart(cartDraft);
            return cart;
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
            lineItemDraft.VariantId = variantId;
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

        public Product CreateProduct(bool cleanOnDispose = true)
        {
            Product product = this.productFixture.CreateProduct(withVariants:false, publish:true);
            if (cleanOnDispose) // if you're not going to update this product
            {
                this.productFixture.ProductsToDelete.Add(product);
            }
            return product;
        }

        public void CleanProductOnDispose(Product product)
        {
            this.productFixture.ProductsToDelete.Add(product);
        }

        public Customer CreateCustomer()
        {
            Customer customer = this.customerFixture.CreateCustomer();
            this.customerFixture.CustomersToDelete.Add(customer);
            return customer;
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

        public ShippingRateDraft GetShippingRateDraftWithPriceTiers()
        {
            ShippingRateDraft rate = new ShippingRateDraft()
            {
                Price = Money.Parse("10.00 EUR"),
                Tiers = this.GetShippingRatePriceTiersAsCartScore()
            };
            return rate;
        }

        public ShippingRateDraft GetShippingRateDraftWithCartClassifications()
        {
            ShippingRateDraft rate = new ShippingRateDraft()
            {
                Price = Money.Parse("10.00 EUR"),
                Tiers = GetShippingRatePriceTiersAsClassification()
            };
            return rate;
        }

        private List<ShippingRatePriceTier> GetShippingRatePriceTiersAsCartScore()
        {
            var shippingRatePriceTiers = new List<ShippingRatePriceTier>();
            shippingRatePriceTiers.Add(new CartScoreShippingRatePriceTier{Score = 0, Price = Money.Parse("10.00 EUR")});
            shippingRatePriceTiers.Add(new CartScoreShippingRatePriceTier{Score = 1, Price = Money.Parse("20.00 EUR")});
            shippingRatePriceTiers.Add(new CartScoreShippingRatePriceTier{Score = 2, Price = Money.Parse("30.00 EUR")});
            return shippingRatePriceTiers;
        }
        private List<ShippingRatePriceTier> GetShippingRatePriceTiersAsClassification()
        {
            var shippingRatePriceTiers = new List<ShippingRatePriceTier>();
            shippingRatePriceTiers.Add(new CartClassificationShippingRatePriceTier{Value = "Small", Price = Money.Parse("20.00 EUR")});
            shippingRatePriceTiers.Add(new CartClassificationShippingRatePriceTier{Value = "Heavy", Price = Money.Parse("30.00 EUR")});
            return shippingRatePriceTiers;
        }
        public ShippingRate GetShippingRate()
        {
            return this.shippingMethodsFixture.GetShippingRate();
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

        public Payment CreatePayment()
        {
            Payment payment = this.paymentsFixture.CreatePayment();
            this.paymentsFixture.PaymentsToDelete.Add(payment);
            return payment;
        }

        public ShoppingList CreateShoppingList()
        {
            ShoppingList shoppingList =
                this.shoppingListFixture.CreateShoppingList(withCustomer: true, withLineItem: true);
            this.shoppingListFixture.ShoppingListToDelete.Add(shoppingList);
            return shoppingList;
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
                StreetName = this.RandomString(10),
                Key = this.RandomString(10)
            };
            return shippingAddress;
        }

        public List<string> GetProjectLanguages()
        {
            return this.projectFixture.GetProjectLanguages();
        }

        public CustomLineItemDraft GetCustomLineItemDraft()
        {
            TaxCategory taxCategory = this.CreateNewTaxCategory();
            var customLineItemDraft = new CustomLineItemDraft
            {
                Name = new LocalizedString() {{"en", this.RandomString(10)}},
                Slug = this.RandomString(10),
                Quantity = this.RandomInt(1,10),
                Money = Money.Parse($"{this.RandomInt(100,10000)} EUR"),
                TaxCategory = new Reference<TaxCategory>() {Id = taxCategory.Id}
            };
            return customLineItemDraft;
        }

        public ItemShippingDetailsDraft GetItemShippingDetailsDraft(string addressKey)
        {
            var itemShippingTarget = this.GetItemShippingTarget(addressKey);
            ItemShippingDetailsDraft itemShippingDetailsDraft = new ItemShippingDetailsDraft
            {
                Targets = new List<ItemShippingTarget>{itemShippingTarget}
            };
            return itemShippingDetailsDraft;
        }

        public List<ItemShippingTarget> GetTargetsDelta(string addressKey)
        {
            var itemShippingTarget = this.GetItemShippingTarget(addressKey);
            List<ItemShippingTarget> targetsDelta = new List<ItemShippingTarget> {itemShippingTarget};
            return targetsDelta;
        }

        public ItemShippingTarget GetItemShippingTarget(string addressKey)
        {
            ItemShippingTarget itemShippingTarget = new ItemShippingTarget
            {
                Quantity = this.RandomInt(1,20),
                AddressKey = addressKey
            };
            return itemShippingTarget;
        }

        public Sdk.Domain.Project.Project SetShippingRateInputTypeToCartScoreForCurrentProject()
        {
            return this.projectFixture.SetShippingRateInputTypeToCartScore();
        }
        public Sdk.Domain.Project.Project SetShippingRateInputTypeToCartClassificationForCurrentProject(List<LocalizedEnumValue> values)
        {
            return this.projectFixture.SetShippingRateInputTypeToCartClassification(values);
        }
        public Sdk.Domain.Project.Project RemoveExistingShippingRateInputTypeForCurrentProject()
        {
            return this.projectFixture.RemoveExistingShippingRateInputType();
        }

    }
}
