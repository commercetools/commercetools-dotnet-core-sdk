using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.CartDiscounts;
using commercetools.Common;
using commercetools.DiscountCodes;
using commercetools.DiscountCodes.UpdateActions;
using commercetools.Project;
using FluentAssertions;
using NUnit.Framework;
using ChangeIsActive = commercetools.DiscountCodes.UpdateActions.ChangeIsActive;
using SetDescription = commercetools.DiscountCodes.UpdateActions.SetDescription;

namespace commercetools.Tests
{
    public class DiscountCodeManagerTests
    {
        private Client _client;
        private DiscountCode _testDiscountCode;

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

            Task<DiscountCode> discountCodeTask =
                Helper.CreateTestDiscountCode(this._project, this._client);
            discountCodeTask.Wait();

            Assert.IsNotNull(discountCodeTask.Result);

            _testDiscountCode = discountCodeTask.Result;
            Assert.NotNull(_testDiscountCode);
            Assert.NotNull(_testDiscountCode.Id);
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            Task<DiscountCode> deleteDiscountCodeTask =
                Helper.DeleteDiscountCode(this._client, _testDiscountCode);
            deleteDiscountCodeTask.Wait();
        }

        /// <summary>
        /// Tests the DiscountCodeManager.GetDiscountCodeByIdAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.GetDiscountCodeByIdAsync"/>
        [Test]
        public async Task ShouldGetDiscountCodeByIdAsync()
        {
            Response<DiscountCode> response = await _client.DiscountCodes().GetDiscountCodeByIdAsync(_testDiscountCode.Id);
            Assert.IsTrue(response.Success);

            DiscountCode discountCode = response.Result;
            Assert.NotNull(discountCode.Id);
            Assert.AreEqual(_testDiscountCode.Id, discountCode.Id);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.QueryDiscountCodesAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.QueryDiscountCodesAsync"/>
        [Test]
        public async Task ShouldQueryDiscountCodeByCodeAsync()
        {
            Response<DiscountCodeQueryResult> response = await _client.DiscountCodes().QueryDiscountCodesAsync("code=\""+_testDiscountCode.Code +"\"");
            Assert.IsTrue(response.Success);

            DiscountCodeQueryResult discountCode = response.Result;
            Assert.NotNull(discountCode);
            Assert.AreEqual(1, discountCode.Count);
            discountCode.Results.ShouldAllBeEquivalentTo(new [] { _testDiscountCode });
        }

        /// <summary>
        /// Tests the DiscountCodeManager.CreateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.CreateDiscountCodeAsync"/>
        [Test]
        public async Task ShouldCreateDiscountCodeAsync()
        {
            // Arrange
            var discountCodeDraft = await Helper.GetDiscountCodeDraft(this._project, this._client);
            var cartDiscountReferences = discountCodeDraft.CartDiscounts.Select(d => new Reference
            {
                Id = d.Id,
                ReferenceType = ReferenceType.CartDiscount
            });
            // Act
            Response<DiscountCode> discountCodeResponse = await _client.DiscountCodes().CreateDiscountCodeAsync(discountCodeDraft);

            // Assert
            var discountCode = discountCodeResponse.Result;
            Assert.IsNotNull(discountCode);
            Assert.IsNotNull(discountCode.Id);
            Assert.AreEqual(discountCodeDraft.CartPredicate, discountCode.CartPredicate);
            Assert.AreEqual(discountCodeDraft.Name.Values, discountCode.Name.Values);
            Assert.AreEqual(discountCodeDraft.Description.Values, discountCode.Description.Values);
            Assert.AreEqual(discountCodeDraft.IsActive, discountCode.IsActive);
            Assert.AreEqual(discountCodeDraft.MaxApplications, discountCode.MaxApplications);
            Assert.AreEqual(discountCodeDraft.MaxApplicationsPerCustomer, discountCode.MaxApplicationsPerCustomer);
            discountCode.CartDiscounts.ShouldAllBeEquivalentTo(cartDiscountReferences);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, discountCode);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.DeleteDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.DeleteDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode)"/>
        [Test]
        public async Task ShouldDeleteDiscountCodeAsync()
        {
            // Arrange            
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);

            // Act
            Response<DiscountCode> discountCodeDeleteResponse = await _client.DiscountCodes().DeleteDiscountCodeAsync(discountCode);


            // Assert
            var deletedtDiscountCode = discountCodeDeleteResponse.Result;
            Assert.IsNotNull(deletedtDiscountCode);
            Assert.IsNotNull(deletedtDiscountCode.Id);
            var getDiscountCodeTask = await this._client.DiscountCodes().GetDiscountCodeByIdAsync(deletedtDiscountCode.Id);

            Assert.AreEqual(404, getDiscountCodeTask.StatusCode);
            Assert.AreEqual(false, getDiscountCodeTask.Success);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.UpdateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.UpdateDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode,commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldSetNameSetDescriptionDiscountCodeAsync()
        {
            // Arrange
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);
            LocalizedString name = new LocalizedString();
            LocalizedString description = new LocalizedString();

            foreach (string language in this._project.Languages)
            {
                string randomPostfix = Helper.GetRandomString(10);
                name.SetValue(language, string.Concat("change-discount-code-name", language, " ", randomPostfix));
                description.SetValue(language, string.Concat("change-discount-code-description", language, "-", randomPostfix));
            }
            var setNameAction = new SetName { Name = name };
            var setDescriptionAction = new SetDescription { Description = description };

            // Act
            var updatedDiscountCodeResponse = await this._client.DiscountCodes()
                .UpdateDiscountCodeAsync(discountCode, new List<UpdateAction> {
                    setNameAction, setDescriptionAction });


            // Assert
            var updatedDiscountCode = updatedDiscountCodeResponse.Result;
            Assert.IsNotNull(updatedDiscountCode);
            Assert.IsNotNull(updatedDiscountCode.Id);
            Assert.AreEqual(updatedDiscountCode.Name.Values, name.Values);
            Assert.AreEqual(updatedDiscountCode.Description.Values, description.Values);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, updatedDiscountCode);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.UpdateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.UpdateDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode,commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldRemoveNameandDescriptionDiscountCodeAsync()
        {
            // Arrange
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);
            var setNameAction = new SetName();
            var setDescriptionAction = new SetDescription();

            // Act
            var updatedDiscountCodeResponse = await this._client.DiscountCodes()
                .UpdateDiscountCodeAsync(discountCode, new List<UpdateAction> {
                    setNameAction, setDescriptionAction });


            // Assert
            var updatedDiscountCode = updatedDiscountCodeResponse.Result;
            Assert.IsNotNull(updatedDiscountCode);
            Assert.IsNotNull(updatedDiscountCode.Id);
            Assert.AreEqual(0, updatedDiscountCode.Name.Values.Count);
            Assert.AreEqual(0, updatedDiscountCode.Description.Values.Count);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, updatedDiscountCode);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.UpdateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.UpdateDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode,commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldSetMaxApplicationsSetMaxApplicationsPerCustomerDiscountCodeAsync()
        {
            // Arrange
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);
            var setMaxApplications = new SetMaxApplications { MaxApplications = Helper.GetRandomNumber(3000, 5000) };
            var setMaxApplicationsPerCustomer = new SetMaxApplicationsPerCustomer { MaxApplicationsPerCustomer = Helper.GetRandomNumber(3000, 5000) };

            // Act
            var updatedDiscountCodeResponse = await this._client.DiscountCodes()
                .UpdateDiscountCodeAsync(discountCode, new List<UpdateAction> {
                    setMaxApplications, setMaxApplicationsPerCustomer });


            // Assert
            var updatedDiscountCode = updatedDiscountCodeResponse.Result;
            Assert.IsNotNull(updatedDiscountCode);
            Assert.IsNotNull(updatedDiscountCode.Id);
            Assert.AreEqual(updatedDiscountCode.MaxApplications, setMaxApplications.MaxApplications);
            Assert.AreEqual(updatedDiscountCode.MaxApplicationsPerCustomer, setMaxApplicationsPerCustomer.MaxApplicationsPerCustomer);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, updatedDiscountCode);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.UpdateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.UpdateDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode,commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldRemoveMaxApplicationsAndMAxApplicationsPerCustomerDiscountCodeAsync()
        {
            // Arrange
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);
            var setMaxApplications = new SetMaxApplications();
            var setMaxApplicationsPerCustomer = new SetMaxApplicationsPerCustomer();

            // Act
            var updatedDiscountCodeResponse = await this._client.DiscountCodes()
                .UpdateDiscountCodeAsync(discountCode, new List<UpdateAction> {
                    setMaxApplications, setMaxApplicationsPerCustomer });


            // Assert
            var updatedDiscountCode = updatedDiscountCodeResponse.Result;
            Assert.IsNotNull(updatedDiscountCode);
            Assert.IsNotNull(updatedDiscountCode.Id);
            Assert.IsNull(updatedDiscountCode.MaxApplications);
            Assert.IsNull(updatedDiscountCode.MaxApplicationsPerCustomer);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, updatedDiscountCode);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.UpdateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.UpdateDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode,commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldChangeCartPredicateDiscountCodeAsync()
        {
            // Arrange
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);
            var setCartPredicate = new SetCartPredicate {CartPredicate = "totalPrice.centAmount > 100000" };

            // Act
            var updatedDiscountCodeResponse = await this._client.DiscountCodes()
                .UpdateDiscountCodeAsync(discountCode, setCartPredicate);


            // Assert
            var updatedDiscountCode = updatedDiscountCodeResponse.Result;
            Assert.IsNotNull(updatedDiscountCode);
            Assert.IsNotNull(updatedDiscountCode.Id);
            Assert.AreEqual(setCartPredicate.CartPredicate, updatedDiscountCode.CartPredicate);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, updatedDiscountCode);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.UpdateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.UpdateDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode,commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldRemoveCartPredicateDiscountCodeAsync()
        {
            // Arrange
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);
            var setCartPredicate = new SetCartPredicate();

            // Act
            var updatedDiscountCodeResponse = await this._client.DiscountCodes()
                .UpdateDiscountCodeAsync(discountCode, setCartPredicate);


            // Assert
            var updatedDiscountCode = updatedDiscountCodeResponse.Result;
            Assert.IsNotNull(updatedDiscountCode);
            Assert.IsNotNull(updatedDiscountCode.Id);
            Assert.IsNull(updatedDiscountCode.CartPredicate);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, updatedDiscountCode);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.UpdateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.UpdateDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode,commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldChangeCartDiscountsDiscountCodeAsync()
        {
            // Arrange
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);
            var oldCartDiscountId = discountCode.CartDiscounts.First().Id;
            var cartDiscount = await Helper.CreateTestCartDiscount(this._project, this._client);
            var reference = new List<Reference>()
            {
                new Reference {ReferenceType = ReferenceType.CartDiscount, Id = cartDiscount.Id}
            };
            var changeCartDiscounts = new ChangeCartDiscounts(reference);

            // Act
            var updatedDiscountCodeResponse = await this._client.DiscountCodes()
                .UpdateDiscountCodeAsync(discountCode, changeCartDiscounts);


            // Assert
            var updatedDiscountCode = updatedDiscountCodeResponse.Result;
            Assert.IsNotNull(updatedDiscountCode);
            Assert.IsNotNull(updatedDiscountCode.Id);
            updatedDiscountCode.CartDiscounts.ShouldAllBeEquivalentTo(changeCartDiscounts.CartDiscounts);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, updatedDiscountCode);
            await this._client.CartDiscounts().DeleteCartDiscountAsync(oldCartDiscountId, 1);
        }

        /// <summary>
        /// Tests the DiscountCodeManager.UpdateDiscountCodeAsync method.
        /// </summary>
        /// <see cref="DiscountCodeManager.UpdateDiscountCodeAsync(commercetools.DiscountCodes.DiscountCode,commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldChangeIsActiveDiscountCodeAsync()
        {
            // Arrange
            var discountCode = await Helper.CreateTestDiscountCode(this._project, this._client);
            var changeIsActive = new ChangeIsActive(!discountCode.IsActive);

            // Act
            var updatedDiscountCodeResponse = await this._client.DiscountCodes()
                .UpdateDiscountCodeAsync(discountCode, changeIsActive);


            // Assert
            var updatedDiscountCode = updatedDiscountCodeResponse.Result;
            Assert.IsNotNull(updatedDiscountCode);
            Assert.IsNotNull(updatedDiscountCode.Id);
            Assert.AreEqual(changeIsActive.IsActive, updatedDiscountCode.IsActive);

            // Cleanup
            await Helper.DeleteDiscountCode(this._client, updatedDiscountCode);
        }
    }
}
