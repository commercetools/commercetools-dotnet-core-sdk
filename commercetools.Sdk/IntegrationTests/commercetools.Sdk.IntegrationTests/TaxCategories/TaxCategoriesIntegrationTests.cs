using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.Domain.TaxCategories.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.TaxCategories.TaxCategoriesFixture;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.TaxCategories.UpdateActions.SetDescriptionUpdateAction;

namespace commercetools.Sdk.IntegrationTests.TaxCategories
{
    [Collection("Integration Tests")]
    public class TaxCategoriesIntegrationTests
    {
        private readonly IClient client;

        public TaxCategoriesIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateTaxCategory()
        {
            var key = $"CreateTaxCategory-{TestingUtility.RandomString()}";
            await WithTaxCategory(
                client, taxCategoryDraft => DefaultTaxCategoryDraftWithKey(taxCategoryDraft, key),
                taxCategory => { Assert.Equal(key, (string) taxCategory.Key); });
        }

        [Fact]
        public async Task GetTaxCategoryById()
        {
            var key = $"GetTaxCategoryById-{TestingUtility.RandomString()}";
            await WithTaxCategory(
                client, taxCategoryDraft => DefaultTaxCategoryDraftWithKey(taxCategoryDraft, key),
                async taxCategory =>
                {
                    var retrievedTaxCategory = await client
                        .ExecuteAsync(new GetByIdCommand<TaxCategory>(taxCategory.Id));
                    Assert.Equal(key, retrievedTaxCategory.Key);
                });
        }

        [Fact]
        public async Task GetTaxCategoryByKey()
        {
            var key = $"GetTaxCategoryByKey-{TestingUtility.RandomString()}";
            await WithTaxCategory(
                client, taxCategoryDraft => DefaultTaxCategoryDraftWithKey(taxCategoryDraft, key),
                async taxCategory =>
                {
                    var retrievedTaxCategory = await client
                        .ExecuteAsync(new GetByKeyCommand<TaxCategory>(taxCategory.Key));
                    Assert.Equal(key, retrievedTaxCategory.Key);
                });
        }

        [Fact]
        public async Task QueryTaxCategories()
        {
            var key = $"QueryTaxCategories-{TestingUtility.RandomString()}";
            await WithTaxCategory(
                client, taxCategoryDraft => DefaultTaxCategoryDraftWithKey(taxCategoryDraft, key),
                async taxCategory =>
                {
                    var queryCommand = new QueryCommand<TaxCategory>();
                    queryCommand.Where(p => p.Key == taxCategory.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteTaxCategoryById()
        {
            var key = $"DeleteTaxCategoryById-{TestingUtility.RandomString()}";
            await WithTaxCategory(
                client, taxCategoryDraft => DefaultTaxCategoryDraftWithKey(taxCategoryDraft, key),
                async taxCategory =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<TaxCategory>(taxCategory));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<TaxCategory>(taxCategory))
                    );
                });
        }

        [Fact]
        public async Task DeleteTaxCategoryByKey()
        {
            var key = $"DeleteTaxCategoryByKey-{TestingUtility.RandomString()}";
            await WithTaxCategory(
                client, taxCategoryDraft => DefaultTaxCategoryDraftWithKey(taxCategoryDraft, key),
                async taxCategory =>
                {
                    await client.ExecuteAsync(
                        new DeleteByKeyCommand<TaxCategory>(taxCategory.Key, taxCategory.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<TaxCategory>(taxCategory))
                    );
                });
        }


        #region UpdateActions

        [Fact]
        public async Task UpdateTaxCategoryChangeName()
        {
            var newName = $"UpdateTaxCategoryChangeName-{TestingUtility.RandomString()}";
            await WithUpdateableTaxCategory(client, async taxCategory =>
            {
                var updateActions = new List<UpdateAction<TaxCategory>>();
                var setKeyAction = new ChangeNameUpdateAction {Name = newName};
                updateActions.Add(setKeyAction);

                var updatedTaxCategory = await client
                    .ExecuteAsync(new UpdateByKeyCommand<TaxCategory>(taxCategory.Key, taxCategory.Version,
                        updateActions));

                Assert.Equal(newName, updatedTaxCategory.Name);
                return updatedTaxCategory;
            });
        }

        [Fact]
        public async Task UpdateTaxCategorySetKey()
        {
            var newKey = $"UpdateTaxCategorySetKey-{TestingUtility.RandomString()}";
            await WithUpdateableTaxCategory(client, async taxCategory =>
            {
                var updateActions = new List<UpdateAction<TaxCategory>>();
                var setKeyAction = new SetKeyUpdateAction {Key = newKey};
                updateActions.Add(setKeyAction);

                var updatedTaxCategory = await client
                    .ExecuteAsync(new UpdateByIdCommand<TaxCategory>(taxCategory, updateActions));

                Assert.Equal(newKey, updatedTaxCategory.Key);
                return updatedTaxCategory;
            });
        }

        [Fact]
        public async Task UpdateTaxCategorySetDescription()
        {
            var newDescription = $"UpdateTaxCategorySetDescription-{TestingUtility.RandomString()}";
            await WithUpdateableTaxCategory(client, async taxCategory =>
            {
                var updateActions = new List<UpdateAction<TaxCategory>>();
                var setKeyAction = new SetDescriptionUpdateAction {Description = newDescription};
                updateActions.Add(setKeyAction);

                var updatedTaxCategory = await client
                    .ExecuteAsync(new UpdateByIdCommand<TaxCategory>(taxCategory, updateActions));

                Assert.Equal(newDescription, updatedTaxCategory.Description);
                return updatedTaxCategory;
            });
        }

        [Fact]
        public async Task UpdateTaxCategoryAddTaxRate()
        {
            var taxRateDraft = GetTaxRateDraft("AG7", "AG", 0.07, true);

            await WithUpdateableTaxCategory(client, async taxCategory =>
            {
                Assert.Empty(taxCategory.Rates);
                var updateActions = new List<UpdateAction<TaxCategory>>();
                var addTaxRateAction = new AddTaxRateUpdateAction {TaxRate = taxRateDraft};
                updateActions.Add(addTaxRateAction);

                var updatedTaxCategory = await client
                    .ExecuteAsync(new UpdateByIdCommand<TaxCategory>(taxCategory, updateActions));

                Assert.Single(updatedTaxCategory.Rates);
                Assert.Equal(taxRateDraft.Name, updatedTaxCategory.Rates[0].Name);
                Assert.Equal(taxRateDraft.Amount, updatedTaxCategory.Rates[0].Amount);
                Assert.Equal(taxRateDraft.Country, updatedTaxCategory.Rates[0].Country);
                return updatedTaxCategory;
            });
        }

        [Fact]
        public async Task UpdateTaxCategoryReplaceTaxRate()
        {
            var taxRateDraft = GetTaxRateDraft("AG7", "AG", 0.07, true);

            await WithUpdateableTaxCategory(client,
                taxCategoryDraft => DefaultTaxCategoryDraftWithTaxRate(taxCategoryDraft, taxRateDraft),
                async taxCategory =>
                {
                    Assert.Single(taxCategory.Rates);
                    taxRateDraft.Amount = taxRateDraft.Amount * 2;

                    var updateActions = new List<UpdateAction<TaxCategory>>();
                    var replaceTaxRateAction = new ReplaceTaxRateUpdateAction
                    {
                        TaxRateId = taxCategory.Rates[0].Id,
                        TaxRate = taxRateDraft
                    };
                    updateActions.Add(replaceTaxRateAction);

                    var updatedTaxCategory = await client
                        .ExecuteAsync(new UpdateByIdCommand<TaxCategory>(taxCategory, updateActions));

                    Assert.Single(updatedTaxCategory.Rates);
                    Assert.Equal(taxRateDraft.Name, updatedTaxCategory.Rates[0].Name);
                    Assert.NotEqual(taxCategory.Rates[0].Amount, updatedTaxCategory.Rates[0].Amount);
                    Assert.Equal(taxRateDraft.Amount, updatedTaxCategory.Rates[0].Amount);
                    return updatedTaxCategory;
                });
        }

        [Fact]
        public async Task UpdateTaxCategoryRemoveTaxRate()
        {
            var taxRateDraft = GetTaxRateDraft("AG7", "AG", 0.07, true);

            await WithUpdateableTaxCategory(client,
                taxCategoryDraft => DefaultTaxCategoryDraftWithTaxRate(taxCategoryDraft, taxRateDraft),
                async taxCategory =>
                {
                    Assert.Single(taxCategory.Rates);

                    var updateActions = new List<UpdateAction<TaxCategory>>();
                    var removeTaxRateAction = new RemoveTaxRateUpdateAction
                    {
                        TaxRateId = taxCategory.Rates[0].Id
                    };
                    updateActions.Add(removeTaxRateAction);

                    var updatedTaxCategory = await client
                        .ExecuteAsync(new UpdateByIdCommand<TaxCategory>(taxCategory, updateActions));

                    Assert.Empty(updatedTaxCategory.Rates);
                    return updatedTaxCategory;
                });
        }

        #endregion
    }
}
