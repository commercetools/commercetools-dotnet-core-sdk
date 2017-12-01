using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// Provides access to the functions in the Subscriptions section of the API.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html"/>
    public class SubscriptionManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/subscriptions";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public SubscriptionManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Retrieves the representation of a subscription by its id.
        /// </summary>
        /// <param name="subscriptionId">Subscription ID</param>
        /// <returns>Subscription</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#get-a-subscription-by-id"/>
        public Task<Response<Subscription>> GetSubscriptionByIdAsync(string subscriptionId)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentException("subscriptionId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", subscriptionId);
            return _client.GetAsync<Subscription>(endpoint);
        }

        /// <summary>
        /// Retrieves the representation of a subscription by its key.
        /// </summary>
        /// <param name="key">Subscription key</param>
        /// <returns>Subscription</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#get-a-subscription-by-key"/>
        public Task<Response<Subscription>> GetSubscriptionByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return _client.GetAsync<Subscription>(endpoint);
        }

        /// <summary>
        /// Queries subscriptions.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>SubscriptionQueryResult</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#query-subscriptions"/>
        public Task<Response<SubscriptionQueryResult>> QuerySubscriptionsAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<SubscriptionQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new subscription.
        /// </summary>
        /// <remarks>
        /// The creation of a Subscription is eventually consistent, it may take up to a minute before it becomes fully active. In order to test that the destination is correctly configured, a test message will be put into the queue. If the message could not be delivered, the subscription will not be created. The payload of the test message is a notification of type ResourceCreated for the resourceTypeId subscription. Currently, a maximum of 25 subscriptions can be created per project.
        /// </remarks>
        /// <param name="subscriptionDraft">SubscriptionDraft object</param>
        /// <returns>Subscription</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#create-a-subscription"/>
        public Task<Response<Subscription>> CreateSubscriptionAsync(SubscriptionDraft subscriptionDraft)
        {
            if (subscriptionDraft == null)
            {
                throw new ArgumentException("subscriptionDraft cannot be null");
            }

            if (subscriptionDraft.Changes == null && subscriptionDraft.Messages == null)
            {
                throw new ArgumentException("Either messages or changes need to be set");
            }

            string payload = JsonConvert.SerializeObject(subscriptionDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<Subscription>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Updates a subscription.
        /// </summary>
        /// <param name="subscription">Subscription</param>
        /// <param name="action">The update action to be performed on the subscription.</param>
        /// <returns>Subscription</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-subscriptions.html#update-subscription"/>
        public Task<Response<Subscription>> UpdateSubscriptionAsync(Subscription subscription, UpdateAction action)
        {
            return UpdateSubscriptionByIdAsync(subscription.Id, subscription.Version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a subscription.
        /// </summary>
        /// <param name="subscription">Subscription</param>
        /// <param name="actions">The list of update actions to be performed on the subscription.</param>
        /// <returns>Subscription</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-subscriptions.html#update-subscription"/>
        public Task<Response<Subscription>> UpdateSubscriptionAsync(Subscription subscription, List<UpdateAction> actions)
        {
            return UpdateSubscriptionByIdAsync(subscription.Id, subscription.Version, actions);
        }

        /// <summary>
        /// Updates a subscription by ID.
        /// </summary>
        /// <param name="subscriptionId">ID of the subscription</param>
        /// <param name="version">The expected version of the subscription on which the changes should be applied. If the expected version does not match the actual version, a 409 Conflict will be returned.</param>
        /// <param name="actions">The list of update actions to be performed on the subscription.</param>
        /// <returns>Subscription</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#update-subscription-by-id"/>
        public Task<Response<Subscription>> UpdateSubscriptionByIdAsync(string subscriptionId, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentException("subscriptionId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", subscriptionId);
            return UpdateSubscriptionAsync(endpoint, version, actions);
        }

        /// <summary>
        /// Updates a subscription by key.
        /// </summary>
        /// <param name="key">Key of the subscription</param>
        /// <param name="version">The expected version of the subscription on which the changes should be applied. If the expected version does not match the actual version, a 409 Conflict will be returned.</param>
        /// <param name="actions">The list of update actions to be performed on the subscription.</param>
        /// <returns>Subscription</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#update-subscription-by-key"/>
        public Task<Response<Subscription>> UpdateSubscriptionByKeyAsync(string key, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return UpdateSubscriptionAsync(endpoint, version, actions);
        }

        /// <summary>
        /// Private worker method for UpdateSubscriptionByIdAsync and UpdateSubscriptionByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="version">The expected version of the subscription on which the changes should be applied. If the expected version does not match the actual version, a 409 Conflict will be returned.</param>
        /// <param name="actions">The list of update actions to be performed on the subscription.</param>
        /// <returns></returns>
        private Task<Response<Subscription>> UpdateSubscriptionAsync(string endpoint, int version, List<UpdateAction> actions)
        {
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

            return _client.PostAsync<Subscription>(endpoint, data.ToString());
        }

        /// <summary>
        /// Deletes a subscription.
        /// </summary>
        /// <param name="subscription">Subscription</param>
        /// <returns>Subscription</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-subscriptions.html#delete-subscription"/>
        public Task<Response<Subscription>> DeleteSubscriptionAsync(Subscription subscription)
        {
            return DeleteSubscriptionByIdAsync(subscription.Id, subscription.Version);
        }

        /// <summary>
        /// Deletes a subscription by its ID.
        /// </summary>
        /// <param name="subscriptionId">Subscription ID</param>
        /// <param name="version">Subscription version</param>
        /// <returns>Subscription</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#delete-subscription-by-id"/>
        public Task<Response<Subscription>> DeleteSubscriptionByIdAsync(string subscriptionId, int version)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentException("Subscription ID is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", subscriptionId);
            return UpdateSubscriptionAsync(endpoint, version);
        }

        /// <summary>
        /// Deletes a subscription by its key.
        /// </summary>
        /// <param name="key">Subscription key</param>
        /// <param name="version">Subscription version</param>
        /// <returns>Subscription</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#delete-subscription-by-id"/>
        public Task<Response<Subscription>> DeleteSubscriptionByKeyAsync(string key, int version)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Subscription ID is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return UpdateSubscriptionAsync(endpoint, version);
        }

        /// <summary>
        /// Private worker method for DeleteSubscriptionByIdAsync and DeleteSubscriptionByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="version">Subscription version</param>
        /// <returns></returns>
        private Task<Response<Subscription>> UpdateSubscriptionAsync(string endpoint, int version)
        {
            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            return _client.DeleteAsync<Subscription>(endpoint, values);
        }

        #endregion
    }
}
