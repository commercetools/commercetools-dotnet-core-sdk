using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories;
using commercetools.Sdk.HttpApi.IntegrationTests.Zones;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ShippingMethods
{
    public class ShippingMethodsFixture : ClientFixture, IDisposable
    {
        public List<ShippingMethod> ShippingMethodsToDelete { get; private set; }

        private readonly TaxCategoryFixture taxCategoryFixture;
        private readonly ZonesFixture zonesFixture;

        public ShippingMethodsFixture() : base()
        {
            this.ShippingMethodsToDelete = new List<ShippingMethod>();
            this.taxCategoryFixture = new TaxCategoryFixture();
            this.zonesFixture = new ZonesFixture();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ShippingMethodsToDelete.Reverse();
            foreach (ShippingMethod shippingMethod in this.ShippingMethodsToDelete)
            {
                ShippingMethod deletedShippingMethod = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<ShippingMethod>(new Guid(shippingMethod.Id), shippingMethod.Version)).Result;
            }
            this.taxCategoryFixture.Dispose();
            this.zonesFixture.Dispose();
        }

        public ShippingMethodDraft GetShippingMethodDraft(string country = null)
        {
            int ran = this.RandomInt();
            TaxCategory taxCategory = this.CreateNewTaxCategory(country);
            ZoneRateDraft zoneRateDraft = this.GetNewZoneRateDraft(country);
            ShippingMethodDraft shippingMethodDraft = new ShippingMethodDraft()
            {
                Name = $"Dhl_{ran}",
                Key = $"Dhl_key_{ran}",
                TaxCategory = new ResourceIdentifier()
                {
                    Id = taxCategory.Id
                }
                ,
                ZoneRates = new List<ZoneRateDraft>()
                {
                    zoneRateDraft
                },
                Description = "DHL"
            };
            return shippingMethodDraft;
        }

        private ZoneRateDraft GetNewZoneRateDraft(string country = null)
        {
            Zone zone = this.CreateNewZone(country);
            ShippingRate shippingRate = this.GetShippingRate();
            ZoneRateDraft zoneRateDraft = new ZoneRateDraft()
            {
                Zone = new ResourceIdentifier()
                {
                    Id = zone.Id
                },
                ShippingRates = new List<ShippingRate>(){ shippingRate }
            };
            return zoneRateDraft;
        }

        public ShippingRate GetShippingRate()
        {
            ShippingRate rate = new ShippingRate()
            {
                Price = Money.Parse("1.00 EUR"),
                FreeAbove = Money.Parse("100.00 EUR")
            };
            return rate;
        }

        public ShippingMethod CreateShippingMethod(string shippingCountry = null)
        {
            return this.CreateShippingMethod(this.GetShippingMethodDraft(shippingCountry));
        }

        public ShippingMethod CreateShippingMethod(ShippingMethodDraft shippingMethodDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            ShippingMethod shippingMethod = commerceToolsClient.ExecuteAsync(new CreateCommand<ShippingMethod>(shippingMethodDraft)).Result;
            return shippingMethod;
        }

        private TaxCategory CreateNewTaxCategory(string country = null)
        {
            TaxCategory taxCategory = this.taxCategoryFixture.CreateTaxCategory(country);
            this.taxCategoryFixture.TaxCategoriesToDelete.Add(taxCategory);
            return taxCategory;
        }
        private Zone CreateNewZone(string country = null)
        {
            Zone zone = this.zonesFixture.CreateZone(country);
            this.zonesFixture.ZonesToDelete.Add(zone);
            return zone;
        }
    }
}
