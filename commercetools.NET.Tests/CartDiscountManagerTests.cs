using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.CartDiscounts;
using commercetools.CartDiscounts.UpdateActions;
using commercetools.Common;
using commercetools.Project;
using FluentAssertions;
using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the CartDiscountManager class, along with some of the cart update actions.
    /// </summary>
    [TestFixture]
    public class CartDiscountManagerTest
    {
        private Client _client;
        private CartDiscount _testCartDiscount;
        private Project.Project _project;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            Assert.IsTrue(_project.Languages.Count > 0);
            Assert.IsTrue(_project.Currencies.Count > 0);

            Task<CartDiscount> cartDiscountTask =
                Helper.CreateTestCartDiscount(this._project, this._client);
            cartDiscountTask.Wait();

            Assert.IsNotNull(cartDiscountTask.Result);

            _testCartDiscount = cartDiscountTask.Result;
            Assert.NotNull(_testCartDiscount);
            Assert.NotNull(_testCartDiscount.Id); 
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            Task task = _client.CartDiscounts().DeleteCartDiscountAsync(_testCartDiscount);
            task.Wait();         
        }

        /// <summary>
        /// Tests the CartDiscountManager.GetCartDiscountByIdAsync method.
        /// </summary>
        /// <see cref="CartDiscountManager.GetCartDiscountByIdAsync"/>
        [Test]
        public async Task ShouldGetCartDiscountByIdAsync()
        {
            Response<CartDiscount> response = await _client.CartDiscounts().GetCartDiscountByIdAsync(_testCartDiscount.Id);
            Assert.IsTrue(response.Success);

            CartDiscount cartDiscount = response.Result;
            Assert.NotNull(cartDiscount.Id);
            Assert.AreEqual(cartDiscount.Id, _testCartDiscount.Id);
        }

        /// <summary>
        /// Tests the CartDiscountManager.CreateCartDiscountAsync method.
        /// </summary>
        /// <see cref="CartDiscountManager.CreateCartDiscountAsync"/>
        [Test]
        public async Task ShouldCreateCartDiscountAsync()
        {
            // Arrange
            CartDiscountDraft cartDiscountDraft = await Helper.GetTestCartDiscountDraft(this._project, this._client, Helper.GetRandomBoolean(),
                Helper.GetRandomBoolean(), "lineItemCount(1 = 1) > 0", "1=1", 5000, false);

            // Act
            Response<CartDiscount> cartDiscountResponse = await _client.CartDiscounts().CreateCartDiscountAsync(cartDiscountDraft);

            // Assert
            var cartDiscount = cartDiscountResponse.Result;
            Assert.IsNotNull(cartDiscount);
            Assert.IsNotNull(cartDiscount.Id);
            Assert.AreEqual(cartDiscountDraft.SortOrder, cartDiscount.SortOrder);
            Assert.AreEqual(cartDiscountDraft.CartPredicate, cartDiscount.CartPredicate);
            Assert.AreEqual(cartDiscountDraft.Name.Values, cartDiscount.Name.Values);
            Assert.AreEqual(cartDiscountDraft.Description.Values, cartDiscount.Description.Values);
            Assert.That(cartDiscountDraft.ValidFrom, Is.EqualTo(cartDiscount.ValidFrom).Within(1).Seconds);
            Assert.That(cartDiscountDraft.ValidUntil, Is.EqualTo(cartDiscount.ValidUntil).Within(1).Seconds);
            Assert.AreEqual(cartDiscountDraft.IsActive, cartDiscount.IsActive);
            Assert.AreEqual(cartDiscountDraft.RequiresDiscountCode, cartDiscount.RequiresDiscountCode);
            cartDiscount.Target.ShouldBeEquivalentTo(cartDiscountDraft.Target);
            cartDiscount.Value.ShouldBeEquivalentTo(cartDiscountDraft.Value);

            // Cleanup
            await _client.CartDiscounts().DeleteCartDiscountAsync(cartDiscount);
        }

        [Test]
        public async Task ShouldChangeCartPredicateCartDiscountAsync()
        {
            // Arrange
            var changePredicateAction = new ChangeCartPredicateAction("lineItemCount(1=1) > 4");
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);

            // Act
            var updatedCartDiscountResponse = await this._client.CartDiscounts()
                .UpdateCartDiscountAsync(cartDiscount, changePredicateAction);


            // Assert
            var updatedCartDiscount = updatedCartDiscountResponse.Result;
            Assert.IsNotNull(updatedCartDiscount);
            Assert.IsNotNull(updatedCartDiscount.Id);
            Assert.AreEqual(updatedCartDiscount.CartPredicate, changePredicateAction.CartPredicate);

            // Cleanup
            await _client.CartDiscounts().DeleteCartDiscountAsync(updatedCartDiscount);
        }

        [Test]
        public async Task ShouldChangeIsActiveChangeRequiresDiscountCodeCartDiscountAsync()
        {
            // Arrange
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);
            var changeActiveAction = new ChangeIsActiveAction(!cartDiscount.IsActive);
            var changeRequiresDiscountCodeAction = new ChangeRequiresDiscountCodeAction(!cartDiscount.RequiresDiscountCode);


            // Act
            var updatedCartDiscountResponse = await this._client.CartDiscounts()
                .UpdateCartDiscountAsync(cartDiscount, new List<UpdateAction> { changeActiveAction, changeRequiresDiscountCodeAction});


            // Assert
            var updatedCartDiscount = updatedCartDiscountResponse.Result;
            Assert.IsNotNull(updatedCartDiscount);
            Assert.IsNotNull(updatedCartDiscount.Id);
            Assert.AreEqual(updatedCartDiscount.IsActive, changeActiveAction.IsActive);
            Assert.AreEqual(updatedCartDiscount.RequiresDiscountCode, changeRequiresDiscountCodeAction.RequiresDiscountCode);

            // Cleanup
            await _client.CartDiscounts().DeleteCartDiscountAsync(updatedCartDiscount);
        }

        [Test]
        public async Task ShouldChangeNameSetDescriptionCartDiscountAsync()
        {
            // Arrange
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);
            LocalizedString name = new LocalizedString();
            LocalizedString description = new LocalizedString();

            foreach (string language in this._project.Languages)
            {
                string randomPostfix = Helper.GetRandomString(10);
                name.SetValue(language, string.Concat("change-cart-discount-name", language, " ", randomPostfix));
                description.SetValue(language, string.Concat("change-cart-discount-description", language, "-", randomPostfix));
            }
            var changeNameAction = new ChangeNameAction(name);
            var setDescriptionAction = new SetDescriptionAction(description);

            // Act
            var updatedCartDiscountResponse = await this._client.CartDiscounts()
                .UpdateCartDiscountAsync(cartDiscount, new List<UpdateAction> {
                    changeNameAction, setDescriptionAction });


            // Assert
            var updatedCartDiscount = updatedCartDiscountResponse.Result;
            Assert.IsNotNull(updatedCartDiscount);
            Assert.IsNotNull(updatedCartDiscount.Id);
            Assert.AreEqual(updatedCartDiscount.Name.Values, name.Values);
            Assert.AreEqual(updatedCartDiscount.Description.Values, description.Values);

            // Cleanup
            await _client.CartDiscounts().DeleteCartDiscountAsync(updatedCartDiscount);
        }

        [Test]
        public async Task ShouldSetValidFromSetValidUntilCartDiscountAsync()
        {
            // Arrange 
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);
            var setValidFrom = new SetValidFromAction(DateTime.UtcNow.AddDays(3));
            var setValidUntil = new SetValidUntilAction(setValidFrom.ValidFrom.Value.AddDays(10));

            // Act
            var updatedCartDiscountResponse = await this._client.CartDiscounts()
                .UpdateCartDiscountAsync(cartDiscount, new List<UpdateAction> {
                    setValidFrom, setValidUntil });


            // Assert
            var updatedCartDiscount = updatedCartDiscountResponse.Result;
            Assert.IsNotNull(updatedCartDiscount);
            Assert.IsNotNull(updatedCartDiscount.Id);
            Assert.That(updatedCartDiscount.ValidFrom, Is.EqualTo(setValidFrom.ValidFrom).Within(1).Seconds);
            Assert.That(updatedCartDiscount.ValidUntil, Is.EqualTo(setValidUntil.ValidUntil).Within(1).Seconds);

            // Cleanup
            await _client.CartDiscounts().DeleteCartDiscountAsync(updatedCartDiscount);
        }

        [Test]
        public async Task ShouldChangeTargetCartDiscountAsync()
        {
            // Arrange 
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);
            var changetarget = new ChangeTargetAction(new CartDiscountTarget(CartDiscountTargetType.CustomLineItems, "money.centAmount > 1000"));

            // Act
            var updatedCartDiscountResponse = await this._client.CartDiscounts()
                .UpdateCartDiscountAsync(cartDiscount, changetarget);


            // Assert
            var updatedCartDiscount = updatedCartDiscountResponse.Result;
            Assert.IsNotNull(updatedCartDiscount);
            Assert.IsNotNull(updatedCartDiscount.Id);
            updatedCartDiscount.Target.ShouldBeEquivalentTo(changetarget.Target);

            // Cleanup
            await _client.CartDiscounts().DeleteCartDiscountAsync(updatedCartDiscount);
        }

        [Test]
        public async Task ShouldChangeValueCartDiscountAsync()
        {
            // Arrange 
            var moneyList = new List<Money>();
            foreach (var currency in this._project.Currencies)
            {
                moneyList.Add(new Money { CentAmount = Helper.GetRandomNumber(100, 1000), CurrencyCode = currency}); 
            }
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);
            var changeValue = new ChangeValueAction(new AbsoluteCartDiscountValue(moneyList));

            // Act
            var updatedCartDiscountResponse = await this._client.CartDiscounts()
                .UpdateCartDiscountAsync(cartDiscount, changeValue);


            // Assert
            var updatedCartDiscount = updatedCartDiscountResponse.Result;
            Assert.IsNotNull(updatedCartDiscount);
            Assert.IsNotNull(updatedCartDiscount.Id);
            updatedCartDiscount.Value.ShouldBeEquivalentTo(changeValue.Value);

            // Cleanup
            await _client.CartDiscounts().DeleteCartDiscountAsync(updatedCartDiscount);
        }

        [Test]
        public async Task ShouldChangeSortOrderCartDiscountAsync()
        {
            // Arrange            
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);
            var cartDiscountDraft = await Helper.GetTestCartDiscountDraft(this._project, this._client, Helper.GetRandomBoolean(),
                Helper.GetRandomBoolean(), "lineItemCount(1 = 1) > 0", "1=1", 5000, false);
            var changeSortOrder = new ChangeSortOrderAction(cartDiscountDraft.SortOrder);

            // Act
            var updatedCartDiscountResponse = await this._client.CartDiscounts()
                .UpdateCartDiscountAsync(cartDiscount, changeSortOrder);


            // Assert
            var updatedCartDiscount = updatedCartDiscountResponse.Result;
            Assert.IsNotNull(updatedCartDiscount);
            Assert.IsNotNull(updatedCartDiscount.Id);
            updatedCartDiscount.SortOrder.ShouldBeEquivalentTo(changeSortOrder.SortOrder);

            // Cleanup
            await _client.CartDiscounts().DeleteCartDiscountAsync(updatedCartDiscount);
        }

        [Test]
        public async Task ShouldDeleteCartDiscountAsync()
        {
            // Arrange            
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);

            // Act
            var updatedCartDiscountResponse = await this._client.CartDiscounts()
                .DeleteCartDiscountAsync(cartDiscount);


            // Assert
            var updatedCartDiscount = updatedCartDiscountResponse.Result;
            Assert.IsNotNull(updatedCartDiscount);
            Assert.IsNotNull(updatedCartDiscount.Id);
            var getCartTask = await this._client.CartDiscounts().GetCartDiscountByIdAsync(updatedCartDiscount.Id);

            Assert.AreEqual(404, getCartTask.StatusCode);
            Assert.AreEqual(false, getCartTask.Success);
            // Cleanup
        }
    }
}
