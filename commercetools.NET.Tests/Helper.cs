using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using commercetools.CartDiscounts;
using commercetools.Carts;
using commercetools.Categories;
using commercetools.Customers;
using commercetools.Common;
using commercetools.DiscountCodes;
using commercetools.Orders;
using commercetools.Payments;
using commercetools.Products;
using commercetools.ProductTypes;
using commercetools.ShippingMethods;
using commercetools.TaxCategories;
using commercetools.Types;
using commercetools.Zones;

using Configuration = commercetools.Common.Configuration;
using ReferenceType = commercetools.Common.ReferenceType;

namespace commercetools.Tests
{
    /// <summary>
    /// Common definitions and methods used for tests.
    /// </summary>
    public class Helper
    {
        private static Random _random = new Random();
		private static Configuration _configuration = null;

        #region Configuration

        /// <summary>
        /// Gets the client configuration from the application settings.
        /// </summary>
        /// <returns>Configuration</returns>
        public static Configuration GetConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = new Configuration(
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.OAuthUrl"]),
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ApiUrl"]),
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ProjectKey"]),
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ClientID"]),
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ClientSecret"]),
                    ProjectScope.ManageProject);
			}

            return _configuration;
        }

        #endregion

        #region Carts

        /// <summary>
        /// Gets a test cart draft.
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="customerId">Customer ID</param>
        /// <returns>CartDraft</returns>
        public static CartDraft GetTestCartDraft(Project.Project project, string customerId = null)
        {
            Address shippingAddress = Helper.GetTestAddress(project);
            Address billingAddress = Helper.GetTestAddress(project);

            string currency = project.Currencies[0];
            string country = project.Countries[0];

            CartDraft cartDraft = new CartDraft(currency);
            cartDraft.Country = country;
            cartDraft.InventoryMode = InventoryMode.None;
            cartDraft.ShippingAddress = shippingAddress;
            cartDraft.BillingAddress = billingAddress;
            cartDraft.DeleteDaysAfterLastModification = GetRandomNumber(1, 10);
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                cartDraft.CustomerId = customerId;
            }
            return cartDraft;
        }

        /// <summary>
        /// Gets a test cart draft with custom line items.
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="customerId">Customer ID</param>
        /// <returns>CartDraft</returns>
        public static CartDraft GetTestCartDraftWithCustomLineItems(Project.Project project, string customerId = null)
        {
            var shippingAddress = GetTestAddress(project);
            var billingAddress = GetTestAddress(project);
            var name = new LocalizedString();
            name.Values.Add("en", "Test-CustomLineItem");
            var currency = project.Currencies[0];
            var country = project.Countries[0];

            var cartDraft = new CartDraft(currency)
            {
                Country = country,
                InventoryMode = InventoryMode.None,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                TaxMode = TaxMode.Disabled,
                CustomLineItems = new List<CustomLineItemDraft>()
            };
            cartDraft.DeleteDaysAfterLastModification = GetRandomNumber(1, 10);

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                cartDraft.CustomerId = customerId;
            }
            cartDraft.CustomLineItems.Add(new CustomLineItemDraft(name)
            {
                Quantity = 40,
                Money = new Money
                {
                    CentAmount = 30,
                    CurrencyCode = currency
                },
                Slug = "Test-CustomLineItem-Slug",
            });
            return cartDraft;
        }

        #endregion

        #region Cart Discounts

        public static async Task<CartDiscountDraft> GetTestCartDiscountDraft(
            Project.Project project, 
            Client client, 
            bool isActive, 
            bool requiresDiscountCode,
            string cartPredicate ,
            string lineItemPredicate,
            int perMyriadAmount,
            bool targetCustomLineItem)
        {
            LocalizedString name = new LocalizedString();
            LocalizedString description = new LocalizedString();

            foreach (string language in project.Languages)
            {
                string randomPostfix = GetRandomString(10);
                name.SetValue(language, string.Concat("test-cart-discount-name", language, " ", randomPostfix));
                description.SetValue(language, string.Concat("test-cart-discount-description", language, "-", randomPostfix));
            }

            CartDiscountQueryResult queryResults;
            string sortOrder;
            do
            {
                sortOrder = GetRandomSortOrder();
                var queryResultsResponse = await client.CartDiscounts().QueryCartDiscountsAsync($"sortOrder=\"{sortOrder}\"");
                queryResults = queryResultsResponse.Result;
            } while (queryResults.Results != null && queryResults.Count > 0);

            return new CartDiscountDraft(
                name,
                new RelativeCartDiscountValue(perMyriadAmount),
                cartPredicate,
                sortOrder,
                requiresDiscountCode)
            {
                Description = description,
                IsActive = isActive,
                ValidFrom = DateTime.UtcNow,
                ValidUntil = GetRandomDateAfter(DateTime.UtcNow.AddDays(100)),
                Target = targetCustomLineItem ?
                            new CartDiscountTarget(CartDiscountTargetType.CustomLineItems, lineItemPredicate) :
                            new CartDiscountTarget(CartDiscountTargetType.LineItems, lineItemPredicate)
            };
        }

        public static async Task<CartDiscount> CreateTestCartDiscount(Project.Project project, Client client)
        {

            var cartDiscountDraft = await GetTestCartDiscountDraft(project, client, GetRandomBoolean(),
                GetRandomBoolean(), "lineItemCount(1 = 1) > 0", "1=1", 5000, false);
            var cartDiscountResponse = await client.CartDiscounts().CreateCartDiscountAsync(cartDiscountDraft);

            return cartDiscountResponse.Result;
        }

        public static async Task<CartDiscount> CreateCartDiscountForCustomLineItems(
            Project.Project project, 
            Client client,
            bool isActive = true,
            bool requiresDiscountCode = false,
            string cartPredicate = "customLineItemCount(1=1) > 0",
            string lineItemPredicate = "1=1",
            int perMyriadAmount = 5000)
        {
            var cartDiscountDraft = await GetTestCartDiscountDraft(project, client, isActive, requiresDiscountCode,
                cartPredicate, lineItemPredicate, 5000, true);
            var cartDiscountResponse = await client.CartDiscounts().CreateCartDiscountAsync(cartDiscountDraft);

            return cartDiscountResponse.Result;
        }

        #endregion

        #region Categories

        /// <summary>
        /// Gets a test category draft.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>CategoryDraft</returns>
        public static CategoryDraft GetTestCategoryDraft(Project.Project project)
        {
            LocalizedString name = new LocalizedString();
            LocalizedString slug = new LocalizedString();
            LocalizedString description = new LocalizedString();
            LocalizedString metaTitle = new LocalizedString();
            LocalizedString metaDescription = new LocalizedString();
            LocalizedString metaKeywords = new LocalizedString();

            foreach (string language in project.Languages)
            {
                string randomPostfix = Helper.GetRandomString(10);
                name.SetValue(language, string.Concat("Test Category ", language, " ", randomPostfix));
                slug.SetValue(language, string.Concat("test-category-", language, "-", randomPostfix));
                description.SetValue(language, string.Concat("Created by commercetools.NET ", language));
                metaTitle.SetValue(language, string.Concat("Category Meta Title ", language));
                metaDescription.SetValue(language, string.Concat("Category Meta Description ", language));
                metaKeywords.SetValue(language, string.Concat("Category Meta Keywords ", language));
            }

            CategoryDraft categoryDraft = new CategoryDraft(name, slug);
            categoryDraft.Description = description;
            categoryDraft.MetaTitle = metaTitle;
            categoryDraft.MetaDescription = metaDescription;
            categoryDraft.MetaKeywords = metaKeywords;

            return categoryDraft;
        }

        #endregion

        #region Common

        /// <summary>
        /// Gets a test address.
        /// </summary>
        /// <returns>Address</returns>
        public static Address GetTestAddress(Project.Project project)
        {
            Address address = new Address();

            address.Title = "Title";
            address.Salutation = "Salutation";
            address.FirstName = "First Name";
            address.LastName = "Last Name";
            address.StreetName = "Main St.";
            address.StreetNumber = "123";
            address.AdditionalStreetInfo = "Additional street info";
            address.PostalCode = "90210";
            address.City = "Los Angeles";
            address.Country = project.Countries.Count > 0 ? project.Countries[0] : "US";
            address.Department = "Dept.";
            address.Building = "Bldg.";
            address.Apartment = "Apt.";
            address.POBox = "P.O. Box";
            address.Phone = "222-333-4444";
            address.Mobile = "222-333-5555";
            address.Email = string.Concat(Helper.GetRandomString(20), "@example.com");
            address.AdditionalAddressInfo = "Additional address info";

            return address;
        }

        /// <summary>
        /// Gets a test money object.
        /// </summary>
        /// <returns>Money</returns>
        public static Money GetTestMoney(Project.Project project)
        {
            Money money = new Money();
            money.CentAmount = Helper.GetRandomNumber(100, 999999);
            money.CurrencyCode = project.Currencies[0];

            return money;
        }

        #endregion

        #region Customers

        /// <summary>
        /// Gets a test customer draft.
        /// </summary>
        /// <returns>CustomerDraft</returns>
        public static CustomerDraft GetTestCustomerDraft()
        {
            string email = string.Concat(Helper.GetRandomString(20), "@example.com");
            string password = Helper.GetRandomString(20);

            CustomerDraft customerDraft = new CustomerDraft(email, password);
            customerDraft.ExternalId = Helper.GetRandomNumber(10000, 99999).ToString();
            customerDraft.Title = "API";
            customerDraft.FirstName = "Test";
            customerDraft.LastName = "Customer";

            return customerDraft;
        }

        #endregion

        #region Discount Codes

        public static async Task<DiscountCode> CreateTestDiscountCode(Project.Project project, Client client)
        {
            LocalizedString name = new LocalizedString();
            LocalizedString description = new LocalizedString();

            foreach (string language in project.Languages)
            {
                string randomPostfix = GetRandomString(10);
                name.SetValue(language, string.Concat("test-discount-code-name", language, " ", randomPostfix));
                description.SetValue(language, string.Concat("test-discount-code-description", language, "-", randomPostfix));
            }
            var cartDiscountDraft = await GetTestCartDiscountDraft(project, client, GetRandomBoolean(),
                GetRandomBoolean(), "lineItemCount(1 = 1) > 0", "1=1", 5000, false);
            var cartDiscountResponse = await client.CartDiscounts().CreateCartDiscountAsync(cartDiscountDraft);
            var discountCodeDraft = new DiscountCodeDraft(
                GetRandomString(10),
                new List<Reference>
                {
                    new Reference {Id = cartDiscountResponse.Result.Id, ReferenceType = ReferenceType.CartDiscount}
                },
                GetRandomBoolean())
            {
                Description = description,
                Name = name,
                MaxApplications = GetRandomNumber(100, 1000),
                MaxApplicationsPerCustomer = GetRandomNumber(100, 1000),
                CartPredicate = "totalPrice.centAmount > 1000"
            };
            var discountCode = await client.DiscountCodes().CreateDiscountCodeAsync(discountCodeDraft);

            return discountCode.Result;
        }

        public static async Task<DiscountCode> DeleteDiscountCode(Client client, DiscountCode discountCode)
        {
            Response<DiscountCode> taskDeleteDiscountCode = await client.DiscountCodes().DeleteDiscountCodeAsync(discountCode);
            var deletedDiscountCode = taskDeleteDiscountCode.Result;

            foreach (var discountCodeCartDiscount in deletedDiscountCode.CartDiscounts)
            {
                await client.CartDiscounts().DeleteCartDiscountAsync(discountCodeCartDiscount.Id, 1);
            }
            return deletedDiscountCode;
        }

        public static async Task<DiscountCodeDraft> GetDiscountCodeDraft(Project.Project project, Client client)
        {
            var name = new LocalizedString();
            var description = new LocalizedString();

            foreach (string language in project.Languages)
            {
                string randomPostfix = Helper.GetRandomString(10);
                name.SetValue(language, string.Concat("test-discount-code-name", language, " ", randomPostfix));
                description.SetValue(language, string.Concat("test-discount-code-description", language, "-", randomPostfix));
            }
            CartDiscount cartDiscount = await Helper.CreateTestCartDiscount(project, client);
            var references = new List<Reference>
            {
                new Reference {Id = cartDiscount.Id, ReferenceType = ReferenceType.CartDiscount}
            };
            var discountCodeDraft = new DiscountCodeDraft(Helper.GetRandomString(10), references, GetRandomBoolean())
            {
                Description = description,
                Name = name,
                MaxApplications = Helper.GetRandomNumber(100, 1000),
                MaxApplicationsPerCustomer = Helper.GetRandomNumber(100, 1000)
            };

            return discountCodeDraft;
        }

        #endregion

        #region Orders

        /// <summary>
        /// Gets a test OrderFromCartDraft.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <returns>OrderFromCartDraft</returns>
        public static OrderFromCartDraft GetTestOrderFromCartDraft(Cart cart)
        {
            OrderFromCartDraft orderFromCartDraft = new OrderFromCartDraft(cart.Id, cart.Version);
            return orderFromCartDraft;
        }

        #endregion 

        #region Payments

        /// <summary>
        /// Gets a test payment draft.
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="customerId">Customer ID</param>
        /// <returns>PaymentDraft</returns>
        public static PaymentDraft GetTestPaymentDraft(Project.Project project, string customerId)
        {
            Reference customerReference = new Reference();
            customerReference.Id = customerId;
            customerReference.ReferenceType = Common.ReferenceType.Customer;

            Money amountPlanned = Helper.GetTestMoney(project);

            PaymentDraft paymentDraft = new PaymentDraft();
            paymentDraft.Customer = customerReference;
            paymentDraft.AmountPlanned = amountPlanned;

            return paymentDraft;
        }

        #endregion

        #region Products

        /// <summary>
        /// Gets a test product draft.
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="productTypeId">Product type ID</param>
        /// <param name="taxCategoryId">Tax category ID</param>
        /// <returns></returns>
        public static ProductDraft GetTestProductDraft(Project.Project project, string productTypeId, string taxCategoryId)
        {
            List<PriceDraft> priceDrafts = new List<PriceDraft>();

            foreach (string currency in project.Currencies)
            {
                Money value = new Money();
                value.CurrencyCode = currency;
                value.CentAmount = Helper.GetRandomNumber(10, 999999);

                priceDrafts.Add(new PriceDraft(value));
            }

            string randomSku = Helper.GetRandomString(10);
            ProductVariantDraft productVariantDraft = new ProductVariantDraft();
            productVariantDraft.Sku = randomSku;
            productVariantDraft.Prices = priceDrafts;

            ResourceIdentifier productType = new ResourceIdentifier();
            productType.Id = productTypeId;
            productType.TypeId = Common.ReferenceType.ProductType;

            LocalizedString name = new LocalizedString();
            LocalizedString slug = new LocalizedString();
            LocalizedString description = new LocalizedString();
            LocalizedString metaTitle = new LocalizedString();
            LocalizedString metaDescription = new LocalizedString();
            LocalizedString metaKeywords = new LocalizedString();

            foreach (string language in project.Languages)
            {
                name.SetValue(language, string.Concat("Test Product ", language, " ", randomSku));
                slug.SetValue(language, string.Concat("test-product-", language, "-", randomSku));
                description.SetValue(language, string.Concat("Created by commercetools.NET ", language));
                metaTitle.SetValue(language, string.Concat("Product Meta Title ", language));
                metaDescription.SetValue(language, string.Concat("Product Meta Description ", language));
                metaKeywords.SetValue(language, string.Concat("Product Meta Keywords ", language));
            }

            Reference taxCategory = new Reference();
            taxCategory.Id = taxCategoryId;
            taxCategory.ReferenceType = Common.ReferenceType.TaxCategory;

            ProductDraft productDraft = new ProductDraft(name, productType, slug);
            productDraft.Key = Helper.GetRandomString(15);
            productDraft.Description = description;
            productDraft.MetaTitle = metaTitle;
            productDraft.MetaDescription = metaDescription;
            productDraft.MetaKeywords = metaKeywords;
            productDraft.TaxCategory = taxCategory;
            productDraft.MasterVariant = productVariantDraft;

            return productDraft;
        }

        #endregion

        #region Product Types

        /// <summary>
        /// Gets a test product type draft.
        /// </summary>
        /// <returns>ProductTypeDraft</returns>
        public static ProductTypeDraft GetTestProductTypeDraft()
        {
            string name = string.Concat("Test Product Type ", Helper.GetRandomString(10));

            ProductTypeDraft draft = new ProductTypeDraft(name, "Created by commercetools.NET");
            draft.Key = Helper.GetRandomString(6);

            return draft;
        }

        #endregion

        #region Shipping Methods

        /// <summary>
        /// Creates a test shipping method draft.
        /// </summary>
        /// <returns>ShippingMethodDraft</returns>
        public static ShippingMethodDraft GetTestShippingMethodDraft(Project.Project project, TaxCategory taxCategory, Zone zone)
        {
            Reference taxCategoryReference = new Reference();
            taxCategoryReference.Id = taxCategory.Id;
            taxCategoryReference.ReferenceType = Common.ReferenceType.TaxCategory;

            string name = string.Concat("Test Shipping Method ", Helper.GetRandomString(10));

            ZoneRate zoneRate = Helper.GetTestZoneRate(project, zone);

            ShippingMethodDraft shippingMethodDraft = new ShippingMethodDraft(name, taxCategoryReference, new List<ZoneRate>() { zoneRate });
            shippingMethodDraft.Description = "Created by commercetools.NET";

            return shippingMethodDraft;
        }

        /// <summary>
        /// Gets a test zone rate.
        /// </summary>
        /// <param name="project">Zone to include</param>
        /// <param name="zone">Zone</param>
        /// <returns>ZoneRate</returns>
        public static ZoneRate GetTestZoneRate(Project.Project project, Zone zone)
        {
            List<ShippingRate> shippingRates = new List<ShippingRate>();

            foreach (string currency in project.Currencies)
            {
                Money money = new Money();
                money.CentAmount = Helper.GetRandomNumber(99, 9999);
                money.CurrencyCode = currency;

                ShippingRate shippingRate = new ShippingRate();
                shippingRate.Price = money;

                shippingRates.Add(shippingRate);
            }

            Reference zoneReference = new Reference();
            zoneReference.Id = zone.Id;
            zoneReference.ReferenceType = Common.ReferenceType.Zone;

            ZoneRate zoneRate = new ZoneRate();
            zoneRate.Zone = zoneReference;
            zoneRate.ShippingRates = shippingRates;

            return zoneRate;
        }

        /// <summary>
        /// Gets a shipping method that has a zone for a specific country.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="country">Country</param>
        /// <returns>Shipping method that has a zone for the specified country, or null if one was not found</returns>
        public static async Task<ShippingMethod> GetShippingMethodForCountry(Client client, string country)
        {
            Response<ShippingMethodQueryResult> shippingMethodQueryResponse = await client.ShippingMethods().QueryShippingMethodsAsync();

            if (!shippingMethodQueryResponse.Success || shippingMethodQueryResponse.Result.Count < 1)
            {
                return null;
            }

            ShippingMethod shippingMethod = null;
            var shippingMethods = shippingMethodQueryResponse.Result.Results.Where(s => s.ZoneRates != null && s.ZoneRates.Count > 0);

            foreach (ShippingMethod s in shippingMethods)
            {
                foreach (ZoneRate zoneRate in s.ZoneRates)
                {
                    Response<Zone> zoneResponse = await client.Zones().GetZoneByIdAsync(zoneRate.Zone.Id);

                    if (zoneResponse.Success && zoneResponse.Result.Locations != null)
                    {
                        foreach (Location location in zoneResponse.Result.Locations)
                        {
                            if (location.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
                            {
                                shippingMethod = s;
                                break;
                            }
                        }
                    }

                    if (shippingMethod != null)
                    {
                        break;
                    }
                }
            }

            return shippingMethod;
        }

        #endregion

        #region Tax Categories

        /// <summary>
        /// Creates a test tax category draft.
        /// </summary>
        /// <returns>TaxCategoryDraft</returns>
        public static TaxCategoryDraft GetTestTaxCategoryDraft(Project.Project project)
        {
            List<TaxRateDraft> taxRateDrafts = new List<TaxRateDraft>();

            foreach (string country in project.Countries)
            {
                string taxRateName = string.Concat("Rate ", country);
                TaxRateDraft taxRateDraft = new TaxRateDraft(taxRateName, true, country);
                taxRateDraft.Amount = ((decimal)Helper.GetRandomNumber(1, 20)) / 100;
                taxRateDrafts.Add(taxRateDraft);
            }

            string name = string.Concat("Test Tax Category ", Helper.GetRandomString(10));

            TaxCategoryDraft taxCategoryDraft = new TaxCategoryDraft(name, taxRateDrafts);
            taxCategoryDraft.Description = "Created by commercetools.NET";

            return taxCategoryDraft;
        }

        #endregion

        #region Types

        /// <summary>
        /// Gets a test type draft.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>TypeDraft</returns>
        public static TypeDraft GetTypeDraft(Project.Project project)
        {
            LocalizedString typeName = new LocalizedString();
            string randomPostfix = Helper.GetRandomString(10);
            typeName.SetValue(project.Languages[0], string.Concat("Test Type", randomPostfix));

            List<string> resourceTypeIds = new List<string> { "order" };

            TypeDraft typeDraft =
                new TypeDraft(string.Concat("test-type-", randomPostfix), typeName, resourceTypeIds);

            typeDraft.FieldDefinitions = new List<FieldDefinition> { 
                Helper.GetFieldDefinition(project, "field1", "Field 1", new commercetools.Types.StringType()),
                Helper.GetFieldDefinition(project, "field2", "Field 2", new commercetools.Types.StringType()),
                Helper.GetFieldDefinition(project, "field3", "Field 3", new commercetools.Types.StringType())
            };

            return typeDraft;
        }

        /// <summary>
        /// Gets a FieldDefinition.
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="name">Field name</param>
        /// <param name="label">Field label</param>
        /// <param name="type">Field type</param>
        /// <returns>FieldDefinition</returns>
        public static FieldDefinition GetFieldDefinition(Project.Project project, string name, string label, FieldType type)
        {
            FieldDefinition fieldDefinition = new FieldDefinition
            {
                Type = type,
                Name = name,
                Required = false
            };

            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.SetValue(project.Languages[0], label);

            return fieldDefinition;
        }

        #endregion

        #region Zones

        /// <summary>
        /// Creates a test shipping method draft.
        /// </summary>
        /// <returns>ShippingMethodDraft</returns>
        public static ZoneDraft GetTestZoneDraft(List<Location> locations = null)
        {
            string name = string.Concat("Test Zone ", Helper.GetRandomString(10));

            ZoneDraft zoneDraft = new ZoneDraft(name);
            
            if (locations != null)
            {
                zoneDraft.Locations = locations;
            }
           
            return zoneDraft;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Gets a random string.
        /// </summary>
        /// <param name="length">Length of string</param>
        /// <returns>Random string</returns>
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Gets a random integer.
        /// </summary>
        /// <param name="minValue">Min. value</param>
        /// <param name="maxValue">Max. value</param>
        /// <returns>Random integer between min. value and max. value</returns>
        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static double GetRandomDouble(int minValue, int maxValue)
        {
            return _random.NextDouble();
        }

        public static string GetRandomSortOrder()
        {
            var order = GetRandomDouble(0, 2).ToString("0.000");
            if (order.EndsWith("0"))
            {
                order = order.TrimEnd('0') + GetRandomNumber(1, 9);
            }
            return order;

        }

        public static DateTime GetRandomDateAfter(DateTime date)
        {
            int days = GetRandomNumber(1, 100);
            return date.AddDays(days);
        }

        public static bool GetRandomBoolean()
        {
            return GetRandomDouble(0, 2) >= 0.5;
        }
        #endregion 
    }
}
