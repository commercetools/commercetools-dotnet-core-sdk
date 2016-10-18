using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Messages;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Customers
{
    /// <summary>
    /// Provides access to the functions in the Customers section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html"/>
    public class CustomerManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/customers";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public CustomerManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a Customer by its ID.
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#get-customer-by-id"/>
        /// <returns>Customer</returns>
        public async Task<Customer> GetCustomerByIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("customerId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", customerId);
            dynamic response = await _client.GetAsync(endpoint);

            return new Customer(response);
        }

        /// <summary>
        /// Queries for Customers.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>CustomerQueryResult</returns>
        public async Task<CustomerQueryResult> QueryCustomersAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
        {
            NameValueCollection values = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(where))
            {
                values.Add("where", where);
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                values.Add("sort", sort);
            }

            if (limit > 0)
            {
                values.Add("limit", limit.ToString());
            }

            if (offset >= 0)
            {
                values.Add("offset", offset.ToString());
            }

            dynamic response = await _client.GetAsync(ENDPOINT_PREFIX, values);

            return new CustomerQueryResult(response);
        }

        /// <summary>
        /// Creates a customer. If an anonymous cart is given then the cart is assigned to the created customer and the version number of the Cart will increase. If the id of an anonymous session is given, all carts and orders will be assigned to the created customer.
        /// </summary>
        /// <param name="customerDraft">CustomerDraft</param>
        /// <returns>CustomerCreatedMessage</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#create-customer-sign-up"/>
        public async Task<CustomerCreatedMessage> CreateCustomerAsync(CustomerDraft customerDraft)
        {
            if (string.IsNullOrWhiteSpace(customerDraft.Email))
            {
                throw new ArgumentException("Email is required");
            }

            if (string.IsNullOrWhiteSpace(customerDraft.Password))
            {
                throw new ArgumentException("Password is required");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string payload = JsonConvert.SerializeObject(customerDraft, settings);
            dynamic response = await _client.PostAsync(ENDPOINT_PREFIX, payload);

            return new CustomerCreatedMessage(response);
        }

        /// <summary>
        /// Updates a Customer.
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="actions">The list of update actions to be performed on the Customer.</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#update-customer"/>
        public async Task<Customer> UpdateCustomerAsync(Customer customer, List<JObject> actions)
        {
            return await UpdateCustomerAsync(customer.Id, customer.Version, actions);
        }

        /// <summary>
        /// Updates a Customer.
        /// </summary>
        /// <param name="customerId">ID of the Customer</param>
        /// <param name="version">The expected version of the Customer on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the Customer.</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#update-customer"/>
        public async Task<Customer> UpdateCustomerAsync(string customerId, int version, List<JObject> actions)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Customer ID is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            if (actions == null || actions.Count < 1)
            {
                throw new ArgumentException("One or more update actions is required");
            }

            JObject data = JObject.FromObject(new
            {
                version = version,
                actions = new JArray(actions.ToArray())
            });

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", customerId);
            dynamic response = await _client.PostAsync(endpoint, data.ToString());

            return new Customer(response);
        }

        /// <summary>
        /// Removes a Customer.
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#delete-customer"/>
        public async Task<Customer> DeleteCustomerAsync(Customer customer)
        {
            return await DeleteCustomerAsync(customer.Id, customer.Version);
        }

        /// <summary>
        /// Removes a Customer.
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <param name="version">Customer version</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#delete-customer"/>
        public async Task<Customer> DeleteCustomerAsync(string customerId, int version)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Customer ID is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", customerId);
            dynamic response = await _client.DeleteAsync(endpoint, values);

            return new Customer(response);
        }

        #endregion
    }
}