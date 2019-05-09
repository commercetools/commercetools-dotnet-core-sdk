using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.HttpApi.IntegrationTests.CartDiscounts;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.DiscountCodes
{
    public class DiscountCodeFixture : ClientFixture, IDisposable
    {
        public List<DiscountCode> DiscountCodesToDelete { get; }

        private readonly CartDiscountsFixture cartDiscountsFixture;

        public DiscountCodeFixture(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            this.DiscountCodesToDelete = new List<DiscountCode>();
            this.cartDiscountsFixture = new CartDiscountsFixture(diagnosticMessageSink);
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.DiscountCodesToDelete.Reverse();
            foreach (DiscountCode discountCode in this.DiscountCodesToDelete)
            {
                DiscountCode deletedType = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<DiscountCode>(new Guid(discountCode.Id),
                        discountCode.Version)).Result;
            }

            this.cartDiscountsFixture.Dispose();
        }

        public DiscountCodeDraft GetDiscountCodeDraft()
        {
            DiscountCodeDraft discountCodeDraft = new DiscountCodeDraft()
            {
                Code = TestingUtility.RandomString(15),
                Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}},
                Description = new LocalizedString() {{"en", TestingUtility.RandomString(20)}},
                IsActive = true,
                CartPredicate = "1 = 1", //for all carts
                ValidFrom = DateTime.Today,
                ValidUntil = DateTime.Today.AddMonths(1), // valid one month
                MaxApplicationsPerCustomer = 2
            };

            CartDiscount cartDiscount = this.CreateCartDiscount();

            discountCodeDraft.CartDiscounts = new List<Reference<CartDiscount>>()
            {
                new Reference<CartDiscount>()
                {
                    Id = cartDiscount.Id,
                }
            };

            return discountCodeDraft;
        }

        public DiscountCode CreateDiscountCode()
        {
            return this.CreateDiscountCode(this.GetDiscountCodeDraft());
        }

        public DiscountCode CreateDiscountCode(string code)
        {
            var discountCodeDraft = this.GetDiscountCodeDraft();
            discountCodeDraft.Code = code;
            return this.CreateDiscountCode(discountCodeDraft);
        }

        public DiscountCode CreateDiscountCode(DiscountCodeDraft discountCodeDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            DiscountCode discountCode = commerceToolsClient
                .ExecuteAsync(new CreateCommand<DiscountCode>(discountCodeDraft)).Result;
            return discountCode;
        }


        private CartDiscount CreateCartDiscount()
        {
            CartDiscount cartDiscount = this.cartDiscountsFixture.CreateCartDiscount(requireDiscountCode: true);
            this.cartDiscountsFixture.CartDiscountsToDelete.Add(cartDiscount);
            return cartDiscount;
        }
    }
}
