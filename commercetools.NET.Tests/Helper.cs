using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

using commercetools.Carts;
using commercetools.Categories;
using commercetools.Customers;
using commercetools.Common;
using commercetools.Orders;
using commercetools.Payments;
using commercetools.Products;
using commercetools.ProductTypes;
using commercetools.ShippingMethods;
using commercetools.TaxCategories;
using commercetools.Zones;

using Configuration = commercetools.Common.Configuration;

namespace commercetools.Tests
{
    /// <summary>
    /// Common definitions and methods used for tests.
    /// </summary>
    public class Helper
    {
        private static Random _random = new Random();

		private static Configuration configuration = null;
        #region Configuration

        /// <summary>
        /// Gets the client configuration from the application settings.
        /// </summary>
        /// <returns>Configuration</returns>
        public static Configuration GetConfiguration()
        {
			if (configuration == null) {
				configuration = new Configuration(
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.OAuthUrl"]),
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ApiUrl"]),
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ProjectKey"]),
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ClientID"]),
                	Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ClientSecret"]),
                ProjectScope.ManageProject);
			}
			return configuration;
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

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                cartDraft.CustomerId = customerId;
            }

            return cartDraft;
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
                name.SetValue(language, string.Concat("Test Category ", language, " ", Helper.GetRandomString(10)));
                slug.SetValue(language, string.Concat("test-category-", language, "-", Helper.GetRandomString(10)));
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

            ProductVariantDraft productVariantDraft = new ProductVariantDraft();
            productVariantDraft.Sku = Helper.GetRandomString(10);
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
                name.SetValue(language, string.Concat("Test Product ", language, " ", Helper.GetRandomString(10)));
                slug.SetValue(language, string.Concat("test-product-", language, "-", Helper.GetRandomString(10)));
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

        /// <summary>
        /// Gets a list of countries for testing.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTestCountries()
        {
            List<string> countries = new List<string>();

            countries.Add("CA");
            countries.Add("CN");
            countries.Add("DE");
            countries.Add("ES");
            countries.Add("GB");
            countries.Add("FR");
            countries.Add("IT");
            countries.Add("JP");
            countries.Add("RU");
            countries.Add("US");
            countries.Add("TR");
            countries.Add("ZA");
           
            return countries;
        }

        #endregion 
    }
}
