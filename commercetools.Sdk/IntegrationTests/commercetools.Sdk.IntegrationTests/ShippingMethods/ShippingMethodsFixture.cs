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
                var taxCategoryRef = new ResourceIdentifier<TaxCategory>
                {
                    Key = taxCategory.Key
                };
                var draftWithTaxCategory = new ShippingMethodDraft {TaxCategory = taxCategoryRef};
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
                var taxCategoryRef = new ResourceIdentifier<TaxCategory>
                {
                    Key = taxCategory.Key
                };
                var draftWithTaxCategory = new ShippingMethodDraft {TaxCategory = taxCategoryRef};
                await WithAsync(client, draftWithTaxCategory, DefaultShippingMethodDraft, func);
            });
        }

        public static async Task WithShippingMethodInUsaZone(IClient client, Func<ShippingMethod, Task> func)
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                var usaLocation = new Location { Country = "US", State = "New York" };
                await WithZone(client, zoneDraft => DefaultZoneWithOneLocation(zoneDraft, usaLocation), async zone =>
                {
                    var zoneRates = new List<ZoneRateDraft>
                    {
                        new ZoneRateDraft
                        {
                            Zone = new ResourceIdentifier<Zone> {Key = zone.Key},
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
                    var taxCategoryRef = new ResourceIdentifier<TaxCategory> {Key = taxCategory.Key};
                    var draftWithTaxCategory = new ShippingMethodDraft
                    {
                        TaxCategory = taxCategoryRef,
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

        #endregion

        #region WithUpdateableShippingMethod

        public static async Task WithUpdateableShippingMethod(IClient client, Func<ShippingMethod, ShippingMethod> func)
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                var taxCategoryRef = new ResourceIdentifier<TaxCategory>
                {
                    Key = taxCategory.Key
                };
                var draftWithTaxCategory = new ShippingMethodDraft {TaxCategory = taxCategoryRef};
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
                var taxCategoryRef = new ResourceIdentifier<TaxCategory>
                {
                    Key = taxCategory.Key
                };
                var draftWithTaxCategory = new ShippingMethodDraft {TaxCategory = taxCategoryRef};
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
                            Zone = new ResourceIdentifier<Zone> {Key = zone.Key},
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
                    var taxCategoryRef = new ResourceIdentifier<TaxCategory> {Key = taxCategory.Key};
                    var draftWithTaxCategory = new ShippingMethodDraft
                    {
                        TaxCategory = taxCategoryRef,
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
                            Zone = new ResourceIdentifier<Zone> {Key = zone.Key}
                        }
                    };
                    var taxCategoryRef = new ResourceIdentifier<TaxCategory> {Key = taxCategory.Key};
                    var draftWithTaxCategory = new ShippingMethodDraft
                    {
                        TaxCategory = taxCategoryRef,
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
