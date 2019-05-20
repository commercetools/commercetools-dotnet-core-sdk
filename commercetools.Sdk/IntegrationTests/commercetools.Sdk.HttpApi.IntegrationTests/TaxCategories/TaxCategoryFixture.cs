using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories
{
    public class TaxCategoryFixture : ClientFixture, IDisposable
    {
        public List<TaxCategory> TaxCategoriesToDelete { get; private set; }

        public TaxCategoryFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.TaxCategoriesToDelete = new List<TaxCategory>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.TaxCategoriesToDelete.Reverse();
            foreach (TaxCategory category in this.TaxCategoriesToDelete)
            {
                TaxCategory deletedCategory = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<TaxCategory>(new Guid(category.Id), category.Version)).Result;
            }
        }

        public TaxCategoryDraft GetTaxCategoryDraft(string country = null, string state = null)
        {
            TaxCategoryDraft taxCategoryDraft = new TaxCategoryDraft()
            {
                Name = TestingUtility.RandomString(10),
                Key = TestingUtility.RandomString(10),
                Rates = new List<TaxRate>(){ this.GetTaxRate(country, state)}
            };
            return taxCategoryDraft;
        }

        public TaxCategory CreateTaxCategory(string country = null, string state = null)
        {
            return this.CreateTaxCategory(this.GetTaxCategoryDraft(country, state));
        }

        public TaxCategory CreateTaxCategory(TaxCategoryDraft taxCategoryDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            TaxCategory taxCategory = commerceToolsClient.ExecuteAsync(new CreateCommand<TaxCategory>(taxCategoryDraft)).Result;
            return taxCategory;
        }

        /// <summary>
        /// Get Tax Rate
        /// </summary>
        /// <returns></returns>
        private TaxRate GetTaxRate(string country = null, string state = null)
        {
            TaxRate taxRate = new TaxRate()
            {
                Name = TestingUtility.RandomString(10),
                Country = country?? "DE",
                State = state,
                Amount = TestingUtility.RandomDouble()
            };
            return taxRate;
        }
    }
}
