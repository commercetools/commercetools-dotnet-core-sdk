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
        public Task<Response<Customer>> GetCustomerByIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("customerId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", customerId);
            return _client.GetAsync<Customer>(endpoint);
        }

        /// <summary>
        /// Queries for Customers.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>CustomerQueryResult</returns>
        public Task<Response<CustomerQueryResult>> QueryCustomersAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<CustomerQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a customer. If an anonymous cart is given then the cart is assigned to the created customer and the version number of the Cart will increase. If the id of an anonymous session is given, all carts and orders will be assigned to the created customer.
        /// </summary>
        /// <param name="customerDraft">CustomerDraft</param>
        /// <returns>CustomerCreatedMessage</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#create-customer-sign-up"/>
        public Task<Response<CustomerCreatedMessage>> CreateCustomerAsync(CustomerDraft customerDraft)
        {
            if (string.IsNullOrWhiteSpace(customerDraft.Email))
            {
                throw new ArgumentException("Email is required");
            }

            if (string.IsNullOrWhiteSpace(customerDraft.Password))
            {
                throw new ArgumentException("Password is required");
            }

            string payload = JsonConvert.SerializeObject(customerDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<CustomerCreatedMessage>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Updates a Customer.
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="action">The  update action to be performed on the Customer.</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#update-customer"/>
        public Task<Response<Customer>> UpdateCustomerAsync(Customer customer, UpdateAction action)
        {
            return UpdateCustomerAsync(customer.Id, customer.Version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a Customer.
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="actions">The list of update actions to be performed on the Customer.</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#update-customer"/>
        public Task<Response<Customer>> UpdateCustomerAsync(Customer customer, List<UpdateAction> actions)
        {
            return UpdateCustomerAsync(customer.Id, customer.Version, actions);
        }

        /// <summary>
        /// Updates a Customer.
        /// </summary>
        /// <param name="customerId">ID of the Customer</param>
        /// <param name="version">The expected version of the Customer on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the Customer.</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#update-customer"/>
        public Task<Response<Customer>> UpdateCustomerAsync(string customerId, int version, List<UpdateAction> actions)
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
                actions = JArray.FromObject(actions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore })
            });

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", customerId);
            return _client.PostAsync<Customer>(endpoint, data.ToString());
        }

        /// <summary>
        /// ChangeCustomersPassword
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="currentPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#change-customers-password"/>
        public Task<Response<Customer>> ChangeCustomersPassword(Customer customer, string currentPassword, string newPassword)
        {
            return ChangeCustomersPassword(customer.Id, customer.Version, currentPassword, newPassword);
        }

        /// <summary>
        /// ChangeCustomersPassword
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="version">Customer version</param>
        /// <param name="currentPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#change-customers-password"/>
        public Task<Response<Customer>> ChangeCustomersPassword(string id, int version, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("id is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                throw new ArgumentException("currentPassword is required");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ArgumentException("newPassword is required");
            }

            JObject data = JObject.FromObject(new
            {
                id = id,
                version = version,
                currentPassword = currentPassword,
                newPassword = newPassword
            });

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/password/");
            return _client.PostAsync<Customer>(endpoint, data.ToString());
        }

        /// <summary>
        /// Retrieves the authenticated customer (a customer that matches the given email/password pair).
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <param name="anonymousCartId">Anonymous cart ID</param>
        /// <param name="anonymousCartSignInMode">AnonymousCartSignInMode</param>
        /// <param name="anonymousId">AnonymousId</param>
        /// <returns>CustomerSignInResult</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-customers.html#authenticate-customer-sign-in"/>
        public Task<Response<CustomerSignInResult>> AuthenticateCustomerAsync(string email, string password, string anonymousCartId = null, AnonymousCartSignInMode? anonymousCartSignInMode = null, string anonymousId = null)
        {
            JObject data = JObject.FromObject(new
            {
                email = email,
                password = password
            });

            if (!string.IsNullOrWhiteSpace(anonymousCartId))
            {
                data.Add(new JProperty("anonymousCartId", anonymousCartId));
            }

            if (anonymousCartSignInMode.HasValue)
            {
                data.Add(new JProperty("anonymousCartSignInMode", anonymousCartSignInMode.Value.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(anonymousId))
            {
                data.Add(new JProperty("anonymousId", anonymousId));
            }

            return _client.PostAsync<CustomerSignInResult>("/login", data.ToString());
        }

        /// <summary>
        /// Removes a Customer.
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#delete-customer"/>
        public Task<Response<Customer>> DeleteCustomerAsync(Customer customer)
        {
            return DeleteCustomerAsync(customer.Id, customer.Version);
        }

        /// <summary>
        /// Removes a Customer.
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <param name="version">Customer version</param>
        /// <returns>Customer</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#delete-customer"/>
        public Task<Response<Customer>> DeleteCustomerAsync(string customerId, int version)
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
            return _client.DeleteAsync<Customer>(endpoint, values);
        }

        #endregion
    }
}
