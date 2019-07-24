using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories;
using commercetools.Sdk.HttpApi.IntegrationTests.Zones;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ShippingMethods
{
    public class ShippingMethodsFixture : ClientFixture, IDisposable
    {
        public List<ShippingMethod> ShippingMethodsToDelete { get; private set; }

        private readonly TaxCategoryFixture taxCategoryFixture;
        private readonly ZonesFixture zonesFixture;

        public ShippingMethodsFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.ShippingMethodsToDelete = new List<ShippingMethod>();
            this.taxCategoryFixture = new TaxCategoryFixture(serviceProviderFixture);
            this.zonesFixture = new ZonesFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ShippingMethodsToDelete.Reverse();
            foreach (ShippingMethod shippingMethod in this.ShippingMethodsToDelete)
            {
                var deletedType = this.TryDeleteResource(shippingMethod).Result;
            }

            this.taxCategoryFixture.Dispose();
            this.zonesFixture.Dispose();
        }

        /// <summary>
        /// Get Shipping Method Draft
        /// </summary>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ShippingMethodDraft GetShippingMethodDraft(string country = null, string state = null)
        {
            int ran = TestingUtility.RandomInt();
            string shippingCountry = country ?? TestingUtility.GetRandomEuropeCountry();
            string shippingState = state ?? $"{shippingCountry}_State_{TestingUtility.RandomInt()}";

            TaxCategory taxCategory = this.CreateNewTaxCategory(shippingCountry, shippingState);
            ZoneRateDraft zoneRateDraft = this.GetNewZoneRateDraft(shippingCountry, shippingState);
            ShippingMethodDraft shippingMethodDraft = new ShippingMethodDraft()
            {
                Name = $"Dhl_{ran}",
                Key = $"Dhl_key_{ran}",
                TaxCategory = new ResourceIdentifier<TaxCategory>
                {
                    Key = taxCategory.Key
                },
                ZoneRates = new List<ZoneRateDraft>()
                {
                    zoneRateDraft
                },
                Description = "DHL"
            };
            return shippingMethodDraft;
        }

        private ZoneRateDraft GetNewZoneRateDraft(string country = null, string state = null)
        {
            Zone zone = this.CreateNewZone(country, state);
            var shippingRate = this.GetShippingRateDraft();
            ZoneRateDraft zoneRateDraft = new ZoneRateDraft()
            {
                Zone = new ResourceIdentifier<Zone>
                {
                    Key = zone.Key
                },
                ShippingRates = new List<ShippingRateDraft>() {shippingRate}
            };
            return zoneRateDraft;
        }

        public ShippingRate GetShippingRate()
        {
            ShippingRate rate = new ShippingRate()
            {
                Price = Money.FromDecimal("EUR", 1),
                FreeAbove = Money.FromDecimal("EUR", 100)
            };
            return rate;
        }
        public ShippingRateDraft GetShippingRateDraft()
        {
            var rate = new ShippingRateDraft
            {
                Price = Money.FromDecimal("EUR", 10),
                FreeAbove = Money.FromDecimal("EUR", 100)
            };
            return rate;
        }

        public ShippingMethod CreateShippingMethod(string shippingCountry = null, string shippingState = null)
        {
            return this.CreateShippingMethod(this.GetShippingMethodDraft(shippingCountry, shippingState));
        }

        public ShippingMethod CreateShippingMethod(ShippingMethodDraft shippingMethodDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            ShippingMethod shippingMethod = commerceToolsClient
                .ExecuteAsync(new CreateCommand<ShippingMethod>(shippingMethodDraft)).Result;
            return shippingMethod;
        }

        private TaxCategory CreateNewTaxCategory(string country = null, string state = null)
        {
            TaxCategory taxCategory = this.taxCategoryFixture.CreateTaxCategory(country, state);
            this.taxCategoryFixture.TaxCategoriesToDelete.Add(taxCategory);
            return taxCategory;
        }

        private Zone CreateNewZone(string country = null, string state = null)
        {
            Zone zone = this.zonesFixture.CreateZone(country, state);
            this.zonesFixture.ZonesToDelete.Add(zone);
            return zone;
        }

        public IReference<TaxCategory> GetShippingMethodTaxCategoryByKey(string shippingMethodKey)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var shippingMethod = commerceToolsClient
                .ExecuteAsync(new GetByKeyCommand<ShippingMethod>(shippingMethodKey)).Result;
            return shippingMethod.TaxCategory;
        }
    }
}
