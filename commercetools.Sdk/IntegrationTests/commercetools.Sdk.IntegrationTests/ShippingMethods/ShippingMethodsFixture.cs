using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.Domain.Zones;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using static commercetools.Sdk.IntegrationTests.TaxCategories.TaxCategoriesFixture;
using static commercetools.Sdk.IntegrationTests.Zones.ZonesFixture;

namespace commercetools.Sdk.IntegrationTests.ShippingMethods
{
    public static class ShippingMethodsFixture
    {
        #region DraftBuilds

        public static ShippingMethodDraft DefaultShippingMethodDraft(ShippingMethodDraft shippingMethodDraft)
        {
            var random = TestingUtility.RandomInt();
            shippingMethodDraft.Key = $"Key_{random}";
            shippingMethodDraft.Name = $"ShippingMethod_{random}";
            return shippingMethodDraft;
        }

        public static ShippingMethodDraft DefaultShippingMethodDraftWithZoneRate(ShippingMethodDraft draft,
            List<ZoneRateDraft> zoneRates)
        {
            var shippingMethodDraft = DefaultShippingMethodDraft(draft);
            shippingMethodDraft.ZoneRates = zoneRates;
            return shippingMethodDraft;
        }

        public static ShippingMethodDraft DefaultShippingMethodDraftWithTaxCategory(ShippingMethodDraft draft,
            IReference<TaxCategory> taxCategoryReference)
        {
            var shippingMethodDraft = DefaultShippingMethodDraft(draft);
            shippingMethodDraft.TaxCategory = taxCategoryReference;
            return shippingMethodDraft;
        }

        public static ShippingMethodDraft DefaultShippingMethodDraftWithKeyWithTaxCategory(ShippingMethodDraft draft,
            IReference<TaxCategory> taxCategoryReference, string key)
        {
            var shippingMethodDraft = DefaultShippingMethodDraft(draft);
            shippingMethodDraft.Key = key;
            shippingMethodDraft.TaxCategory = taxCategoryReference;
            return shippingMethodDraft;
        }

        #endregion

        #region WithShippingMethod

        public static async Task WithShippingMethod(IClient client, Action<ShippingMethod> func)
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                var draftWithTaxCategory = new ShippingMethodDraft
                {
                    TaxCategory = taxCategory.ToKeyResourceIdentifier()
                };
                await With(client, draftWithTaxCategory, DefaultShippingMethodDraft, func);
            });
        }

        public static async Task WithShippingMethod(IClient client,
            Func<ShippingMethodDraft, ShippingMethodDraft> draftAction, Action<ShippingMethod> func)
        {
            await With(client, new ShippingMethodDraft(), draftAction, func);
        }

        public static async Task WithShippingMethod(IClient client, Func<ShippingMethod, Task> func)
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                var draftWithTaxCategory = new ShippingMethodDraft
                {
                    TaxCategory = taxCategory.ToKeyResourceIdentifier()
                };
                await WithAsync(client, draftWithTaxCategory, DefaultShippingMethodDraft, func);
            });
        }

        public static async Task WithShippingMethodInUsaZone(IClient client, Func<ShippingMethod, Task> func)
        {
            var usaLocation = new Location {Country = "US", State = "New York"};
            var address = new Address
            {
                Country = usaLocation.Country,
                State = usaLocation.State
            };
            var taxRateDraft = TestingUtility.GetTaxRateDraft(address);
            await WithTaxCategory(client,
                draft => DefaultTaxCategoryDraftWithTaxRate(draft, taxRateDraft),
                async taxCategory =>
                {
                    await WithZone(client, zoneDraft => DefaultZoneWithOneLocation(zoneDraft, usaLocation),
                        async zone =>
                        {
                            var zoneRates = new List<ZoneRateDraft>
                            {
                                new ZoneRateDraft
                                {
                                    Zone = zone.ToKeyResourceIdentifier(),
                                    ShippingRates = new List<ShippingRateDraft>
                                    {
                                        new ShippingRateDraft
                                        {
                                            Price = Money.FromDecimal("USD", 12),
                                            FreeAbove = Money.FromDecimal("USD", 120)
                                        }
                                    }
                                }
                            };
                            var draftWithTaxCategory = new ShippingMethodDraft
                            {
                                TaxCategory = taxCategory.ToKeyResourceIdentifier(),
                                ZoneRates = zoneRates
                            };
                            await WithAsync(client, draftWithTaxCategory, DefaultShippingMethodDraft, func);
                        });
                });
        }

        public static async Task WithShippingMethod(IClient client,
            Func<ShippingMethodDraft, ShippingMethodDraft> draftAction, Func<ShippingMethod, Task> func)
        {
            await WithAsync(client, new ShippingMethodDraft(), draftAction, func);
        }

        public static async Task WithShippingMethodWithZoneRateAndTaxCategory(IClient client,
            Func<ShippingMethodDraft, ShippingMethodDraft> draftAction, Address address,
            Func<ShippingMethod, Task> func)
        {
            var taxRateDraft = TestingUtility.GetTaxRateDraft(address);
            var zoneLocation = new Location {Country = address.Country, State = address.State};

            await WithZone(client, draft => DefaultZoneWithOneLocation(draft, zoneLocation),
                async zone =>
                {
                    await WithTaxCategory(client, draft =>
                            DefaultTaxCategoryDraftWithTaxRate(draft, taxRateDraft),
                        async taxCategory =>
                        {
                            var shippingRate = TestingUtility.GetShippingRateDraft();
                            var zoneRateDraft = new ZoneRateDraft()
                            {
                                Zone = zone.ToKeyResourceIdentifier(),
                                ShippingRates = new List<ShippingRateDraft>() {shippingRate}
                            };

                            var shippingMethodDraft = DefaultShippingMethodDraftWithTaxCategory(
                                new ShippingMethodDraft(), taxCategory.ToKeyResourceIdentifier());
                            shippingMethodDraft.ZoneRates = new List<ZoneRateDraft>
                            {
                                zoneRateDraft
                            };

                            await WithAsync(client, shippingMethodDraft, draftAction, func);
                        });
                });
        }

        #endregion

        #region WithUpdateableShippingMethod

        public static async Task WithUpdateableShippingMethod(IClient client, Func<ShippingMethod, ShippingMethod> func)
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                var draftWithTaxCategory = new ShippingMethodDraft
                    {TaxCategory = taxCategory.ToKeyResourceIdentifier()};
                await WithUpdateable(client, draftWithTaxCategory, DefaultShippingMethodDraft, func);
            });
        }

        public static async Task WithUpdateableShippingMethod(IClient client,
            Func<ShippingMethodDraft, ShippingMethodDraft> draftAction, Func<ShippingMethod, ShippingMethod> func)
        {
            await WithUpdateable(client, new ShippingMethodDraft(), draftAction, func);
        }

        public static async Task WithUpdateableShippingMethod(IClient client,
            Func<ShippingMethod, Task<ShippingMethod>> func)
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                var draftWithTaxCategory = new ShippingMethodDraft
                    {TaxCategory = taxCategory.ToKeyResourceIdentifier()};
                await WithUpdateableAsync(client, draftWithTaxCategory, DefaultShippingMethodDraft, func);
            });
        }

        public static async Task WithUpdateableShippingMethodWithZoneRate(IClient client,
            Func<ShippingMethod, Task<ShippingMethod>> func)
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithZone(client, async zone =>
                {
                    var zoneRates = new List<ZoneRateDraft>
                    {
                        new ZoneRateDraft
                        {
                            Zone = zone.ToKeyResourceIdentifier(),
                            ShippingRates = new List<ShippingRateDraft>
                            {
                                new ShippingRateDraft
                                {
                                    Price = Money.FromDecimal("EUR", 10),
                                    FreeAbove = Money.FromDecimal("EUR", 100)
                                }
                            }
                        }
                    };
                    var draftWithTaxCategory = new ShippingMethodDraft
                    {
                        TaxCategory = taxCategory.ToKeyResourceIdentifier(),
                        ZoneRates = zoneRates
                    };
                    await WithUpdateableAsync(client, draftWithTaxCategory, DefaultShippingMethodDraft, func);
                });
            });
        }

        public static async Task WithUpdateableShippingMethodWithZone(IClient client,
            Func<ShippingMethod, Task<ShippingMethod>> func)
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithZone(client, async zone =>
                {
                    var zoneRates = new List<ZoneRateDraft>
                    {
                        new ZoneRateDraft
                        {
                            Zone = zone.ToKeyResourceIdentifier()
                        }
                    };
                    var draftWithTaxCategory = new ShippingMethodDraft
                    {
                        TaxCategory = taxCategory.ToKeyResourceIdentifier(),
                        ZoneRates = zoneRates
                    };
                    await WithUpdateableAsync(client, draftWithTaxCategory, DefaultShippingMethodDraft, func);
                });
            });
        }

        public static async Task WithUpdateableShippingMethod(IClient client,
            Func<ShippingMethodDraft, ShippingMethodDraft> draftAction, Func<ShippingMethod, Task<ShippingMethod>> func)
        {
            await WithUpdateableAsync(client, new ShippingMethodDraft(), draftAction, func);
        }

        #endregion
    }
}