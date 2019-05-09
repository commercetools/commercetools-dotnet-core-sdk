using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using Xunit.Abstractions;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.CartDiscounts.SetDescriptionUpdateAction;

namespace commercetools.Sdk.HttpApi.IntegrationTests.CartDiscounts
{
    [Collection("Integration Tests")]
    public class CartDiscountsIntegrationTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly CartDiscountsFixture cartDiscountFixture;

        public CartDiscountsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.cartDiscountFixture = new CartDiscountsFixture(serviceProviderFixture);
        }

        [Fact]
        public void CreateCartDiscount()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscountDraft cartDiscountDraft = this.cartDiscountFixture.GetCartDiscountDraft();
            CartDiscount cartDiscount = commerceToolsClient
                .ExecuteAsync(new CreateCommand<CartDiscount>(cartDiscountDraft)).Result;
            this.cartDiscountFixture.CartDiscountsToDelete.Add(cartDiscount);
            Assert.Equal(cartDiscountDraft.Name["en"], cartDiscount.Name["en"]);
        }

        [Fact]
        public void GetCartDiscountById()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();
            this.cartDiscountFixture.CartDiscountsToDelete.Add(cartDiscount);
            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<CartDiscount>(new Guid(cartDiscount.Id))).Result;
            Assert.Equal(cartDiscount.Id, retrievedCartDiscount.Id);
        }

        [Fact]
        public void QueryCartDiscounts()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();
            this.cartDiscountFixture.CartDiscountsToDelete.Add(cartDiscount);
            QueryPredicate<CartDiscount> queryPredicate =
                new QueryPredicate<CartDiscount>(cd => cd.Id == cartDiscount.Id.valueOf());
            QueryCommand<CartDiscount> queryCommand = new QueryCommand<CartDiscount>();
            queryCommand.SetWhere(queryPredicate);
            PagedQueryResult<CartDiscount> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, cd => cd.Id == cartDiscount.Id);
        }

        [Fact]
        public async void DeleteCartDiscountById()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();
            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version))
                .Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<CartDiscount>(new Guid(retrievedCartDiscount.Id))));
            Assert.Equal(404, exception.StatusCode);
        }

        #region UpdateActions

        [Fact]
        public void UpdateCartDiscountByIdChangeValue()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            //creating new cart discount value
            var newCartDiscountValue = this.cartDiscountFixture.GetCartDiscountValueAsAbsolute();

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            ChangeValueUpdateAction changeValueUpdateAction = new ChangeValueUpdateAction()
                {Value = newCartDiscountValue};
            updateActions.Add(changeValueUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id),
                    cartDiscount.Version, updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);

            var cardDiscountAmount = ((AbsoluteCartDiscountValue) cartDiscount.Value).Money[0].CentAmount;
            var retrievedCartDiscountAmount =
                ((AbsoluteCartDiscountValue) retrievedCartDiscount.Value).Money[0].CentAmount;

            Assert.NotEqual(cartDiscount.Version, retrievedCartDiscount.Version);
            Assert.NotEqual(cardDiscountAmount, retrievedCartDiscountAmount);
        }

        [Fact]
        public void UpdateCartDiscountByIdChangeCartPredicate()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            //creating new cart discount value
            var newCartPredicate = "1 <> 1";

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            ChangeCartPredicateUpdateAction changeValueUpdateAction = new ChangeCartPredicateUpdateAction()
                {CartPredicate = newCartPredicate};
            updateActions.Add(changeValueUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id),
                    cartDiscount.Version, updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);

            Assert.NotEqual(cartDiscount.Version, retrievedCartDiscount.Version);
            Assert.NotEqual(cartDiscount.CartPredicate, retrievedCartDiscount.CartPredicate);
        }

        [Fact]
        public void UpdateCartDiscountByIdChangeTarget()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            //creating new cart discount value
            var newCartDiscountTarget = new LineItemsCartDiscountTarget()
            {
                Predicate = " 1 <> 1"
            };

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            ChangeTargetUpdateAction changeTargetUpdateAction = new ChangeTargetUpdateAction()
                {Target = newCartDiscountTarget};
            updateActions.Add(changeTargetUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id),
                    cartDiscount.Version, updateActions))
                .Result;

            var cartDiscountTarget = (LineItemsCartDiscountTarget) cartDiscount.Target;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);

            Assert.NotEqual(cartDiscount.Version, retrievedCartDiscount.Version);
            Assert.NotEqual(cartDiscountTarget.Predicate, newCartDiscountTarget.Predicate);
        }

        [Fact]
        public void UpdateCartDiscountByIdChangeIsActive()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            ChangeIsActiveUpdateAction changeIsActiveUpdateAction = new ChangeIsActiveUpdateAction()
            {
                IsActive = !cartDiscount.IsActive
            };
            updateActions.Add(changeIsActiveUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);
            Assert.NotEqual(retrievedCartDiscount.IsActive, cartDiscount.IsActive);
        }

        [Fact]
        public void UpdateCartDiscountByIdChangeName()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            string name = TestingUtility.RandomString(10);
            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            ChangeNameUpdateAction changeNameUpdateAction = new ChangeNameUpdateAction()
            {
                Name = new LocalizedString() {{"en", name}}
            };
            updateActions.Add(changeNameUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);
            Assert.Equal(name, retrievedCartDiscount.Name["en"]);
        }

        [Fact]
        public void UpdateCartDiscountByIdSetDescription()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            string newDescription = TestingUtility.RandomString(20);
            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            SetDescriptionUpdateAction setDescriptionUpdateAction = new SetDescriptionUpdateAction()
            {
                Description = new LocalizedString() {{"en", newDescription}}
            };
            updateActions.Add(setDescriptionUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);
            Assert.Equal(newDescription, retrievedCartDiscount.Description["en"]);
        }

        [Fact]
        public void UpdateCartDiscountByIdChangeSortOrder()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            ChangeSortOrderUpdateAction changeSortOrderUpdateAction = new ChangeSortOrderUpdateAction()
            {
                SortOrder = TestingUtility.RandomSortOrder()
            };
            updateActions.Add(changeSortOrderUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);
            Assert.NotEqual(retrievedCartDiscount.SortOrder, cartDiscount.SortOrder);
        }

        [Fact]
        public void UpdateCartDiscountByIdChangeRequiresDiscountCode()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            ChangeRequiresDiscountCodeUpdateAction changeRequiresDiscountCode =
                new ChangeRequiresDiscountCodeUpdateAction()
                {
                    RequiresDiscountCode = !cartDiscount.RequiresDiscountCode
                };
            updateActions.Add(changeRequiresDiscountCode);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);
            Assert.NotEqual(retrievedCartDiscount.RequiresDiscountCode, cartDiscount.RequiresDiscountCode);
        }

        [Fact]
        public void UpdateCartDiscountByIdSetValidFrom()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            SetValidFromUpdateAction setValidFromUpdateAction = new SetValidFromUpdateAction()
            {
                ValidFrom = DateTime.Today.AddMinutes(1)
            };
            updateActions.Add(setValidFromUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);
            Assert.NotEqual(retrievedCartDiscount.ValidFrom, cartDiscount.ValidFrom);
        }

        [Fact]
        public void UpdateCartDiscountByIdSetValidUntil()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            SetValidUntilUpdateAction setValidUntilUpdateAction = new SetValidUntilUpdateAction()
            {
                ValidUntil = DateTime.Today.AddDays(1)
            };
            updateActions.Add(setValidUntilUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);
            Assert.NotEqual(retrievedCartDiscount.ValidUntil, cartDiscount.ValidUntil);
        }

        [Fact]
        public void UpdateCartDiscountByIdSetValidFromAndUntil()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            SetValidFromAndUntilUpdateAction setValidUntilUpdateAction = new SetValidFromAndUntilUpdateAction()
            {
                ValidFrom = cartDiscount.ValidFrom.AddDays(2),
                ValidUntil = cartDiscount.ValidUntil.AddDays(2)
            };
            updateActions.Add(setValidUntilUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);

            Assert.NotEqual(retrievedCartDiscount.ValidFrom, cartDiscount.ValidFrom);
            Assert.NotEqual(retrievedCartDiscount.ValidUntil, cartDiscount.ValidUntil);
        }

        [Fact]
        public void UpdateCartDiscountByIdChangeStackingMode()
        {
            IClient commerceToolsClient = this.cartDiscountFixture.GetService<IClient>();
            CartDiscount cartDiscount = this.cartDiscountFixture.CreateCartDiscount();

            List<UpdateAction<CartDiscount>> updateActions = new List<UpdateAction<CartDiscount>>();
            ChangeStackingModeUpdateAction changeStackingModeUpdateAction = new ChangeStackingModeUpdateAction()
            {
                StackingMode = StackingMode.StopAfterThisDiscount
            };
            updateActions.Add(changeStackingModeUpdateAction);

            CartDiscount retrievedCartDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(new Guid(cartDiscount.Id), cartDiscount.Version,
                    updateActions))
                .Result;

            this.cartDiscountFixture.CartDiscountsToDelete.Add(retrievedCartDiscount);
            Assert.NotEqual(retrievedCartDiscount.StackingMode, cartDiscount.StackingMode);
        }

        #endregion
    }
}
