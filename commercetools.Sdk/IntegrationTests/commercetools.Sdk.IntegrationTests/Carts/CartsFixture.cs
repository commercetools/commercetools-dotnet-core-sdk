using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.ShippingMethods;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using Type = commercetools.Sdk.Domain.Types.Type;

namespace commercetools.Sdk.IntegrationTests.Carts
{
    public static class CartsFixture
    {
        #region Fields
        public const string DefaultCurrency = "EUR";

        #endregion

        #region DraftBuilds

        public static CartDraft DefaultCartDraft(CartDraft cartDraft)
        {
            var randomInt = TestingUtility.RandomInt();
            cartDraft.Currency = DefaultCurrency;
            return cartDraft;
        }
        public static CartDraft DefaultCartDraftWithTaxMode(CartDraft draft, TaxMode taxMode)
        {
            var cartDraft = DefaultCartDraft(draft);
            cartDraft.TaxMode = taxMode;
            return cartDraft;
        }
        
        public static CartDraft DefaultCartDraftWithTaxRoundingMode(CartDraft draft, RoundingMode roundingMode)
        {
            var cartDraft = DefaultCartDraft(draft);
            cartDraft.TaxRoundingMode = roundingMode;
            return cartDraft;
        }
        public static CartDraft DefaultCartDraftWithTaxCalculationMode(CartDraft draft, TaxCalculationMode taxCalculationMode)
        {
            var cartDraft = DefaultCartDraft(draft);
            cartDraft.TaxCalculationMode = taxCalculationMode;
            return cartDraft;
        }
        
        public static CartDraft DefaultCartDraftWithCustomer(CartDraft draft, Customer customer)
        {
            var cartDraft = DefaultCartDraft(draft);
            cartDraft.CustomerId = customer.Id;
            cartDraft.CustomerEmail = customer.Email;
            return cartDraft;
        }

        public static CartDraft DefaultCartDraftWithShippingAddress(CartDraft draft, Address address)
        {
            var cartDraft = DefaultCartDraft(draft);
            cartDraft.ShippingAddress = address;
            return cartDraft;
        }
        public static CartDraft DefaultCartDraftWithShippingMethod(CartDraft draft, ShippingMethod shippingMethod)
        {
            var cartDraft = DefaultCartDraft(draft);
            cartDraft.ShippingMethod = shippingMethod.ToReference();
            return cartDraft;
        }
        
        public static CartDraft DefaultCartDraftWithCustomType(CartDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var cartDraft = DefaultCartDraft(draft);
            cartDraft.Custom = customFieldsDraft;

            return cartDraft;
        }
        #endregion

        #region WithCart

        public static async Task WithCart( IClient client, Action<Cart> func)
        {
            await With(client, new CartDraft(), DefaultCartDraft, func);
        }
        public static async Task WithCart( IClient client, Func<CartDraft, CartDraft> draftAction, Action<Cart> func)
        {
            await With(client, new CartDraft(), draftAction, func);
        }

        public static async Task WithCart( IClient client, Func<Cart, Task> func)
        {
            await WithAsync(client, new CartDraft(), DefaultCartDraft, func);
        }
        public static async Task WithCart( IClient client, Func<CartDraft, CartDraft> draftAction, Func<Cart, Task> func)
        {
            await WithAsync(client, new CartDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableCart

        public static async Task WithUpdateableCart(IClient client, Func<Cart, Cart> func)
        {
            await WithUpdateable(client, new CartDraft(), DefaultCartDraft, func);
        }

        public static async Task WithUpdateableCart(IClient client, Func<CartDraft, CartDraft> draftAction, Func<Cart, Cart> func)
        {
            await WithUpdateable(client, new CartDraft(), draftAction, func);
        }

        public static async Task WithUpdateableCart(IClient client, Func<Cart, Task<Cart>> func)
        {
            await WithUpdateableAsync(client, new CartDraft(), DefaultCartDraft, func);
        }
        public static async Task WithUpdateableCart(IClient client, Func<CartDraft, CartDraft> draftAction, Func<Cart, Task<Cart>> func)
        {
            await WithUpdateableAsync(client, new CartDraft(), draftAction, func);
        }

        #endregion   
    }
}