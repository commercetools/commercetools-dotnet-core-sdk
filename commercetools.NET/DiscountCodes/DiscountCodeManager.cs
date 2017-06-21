using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using commercetools.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.DiscountCodes
{
    public class DiscountCodeManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/discount-codes";

        #endregion

        #region Member Variables

        private readonly Client _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public DiscountCodeManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a Discount Code by its ID.
        /// </summary>
        /// <param name="discountCodeId">Discount Code ID</param>
        /// <see href="https://dev.commercetools.com/http-api-projects-discountCodes.html#get-discountcode-by-id"/>
        /// <returns>DiscountCode</returns>
        public Task<Response<DiscountCode>> GetDiscountCodeByIdAsync(string discountCodeId)
        {
            if (string.IsNullOrWhiteSpace(discountCodeId))
            {
                throw new ArgumentException($"{nameof(discountCodeId)} is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", discountCodeId);
            return _client.GetAsync<DiscountCode>(endpoint);
        }

        /// <summary>
        /// Queries for DiscountCode.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>DiscountCodeQueryResult</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-discountCodes.html#query-discountcodes"/>
        public Task<Response<DiscountCodeQueryResult>> QueryDiscountCodesAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<DiscountCodeQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new Discount Code.
        /// </summary>
        /// <param name="discountCodeDraft">DiscountCodeDraft</param>
        /// <returns>DiscountCode</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-discountCodes.html#create-a-discountcode"/>
        public Task<Response<DiscountCode>> CreateDiscountCodeAsync(DiscountCodeDraft discountCodeDraft)
        {
            string payload = JsonConvert.SerializeObject(discountCodeDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<DiscountCode>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Removes a Discount Code.
        /// </summary>
        /// <param name="discountCode">DiscountCode</param>
        /// <returns>DiscountCode</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-discountCodes.html#delete-discountcode"/>
        public Task<Response<DiscountCode>> DeleteDiscountCodeAsync(DiscountCode discountCode)
        {
            return DeleteDiscountCodeAsync(discountCode.Id, discountCode.Version);
        }

        /// <summary>
        /// Removes a DiscountCode.
        /// </summary>
        /// <param name="discountCartId">DiscountCode ID</param>
        /// <param name="version">DiscountCode version</param>
        /// <returns>DiscountCode</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-discountCodes.html#delete-discountcode"/>
        public Task<Response<DiscountCode>> DeleteDiscountCodeAsync(string discountCartId, int version)
        {
            if (string.IsNullOrWhiteSpace(discountCartId))
            {
                throw new ArgumentException($"{nameof(discountCartId)} is required");
            }

            if (version < 1)
            {
                throw new ArgumentException($"{nameof(version)} is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", discountCartId);
            return _client.DeleteAsync<DiscountCode>(endpoint, values);
        }

        /// <summary>
        /// Updates a Discount Code.
        /// </summary>
        /// <param name="discountCode">DiscountCode</param>
        /// <param name="action">The update action to be performed on the discount code.</param>
        /// <returns>DiscountCode</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-discountCodes.html#update-discountcode"/>
        public Task<Response<DiscountCode>> UpdateDiscountCodeAsync(DiscountCode discountCode, UpdateAction action)
        {
            return UpdateDiscountCodeAsync(discountCode.Id, discountCode.Version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a Discount Code.
        /// </summary>
        /// <param name="discountCode">DiscountCode</param>
        /// <param name="actions">The list of update actions to be performed on the cart discount.</param>
        /// <returns>DiscountCode</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-discountCodes.html#update-discountcode"/>
        public Task<Response<DiscountCode>> UpdateDiscountCodeAsync(DiscountCode discountCode, List<UpdateAction> actions)
        {
            return UpdateDiscountCodeAsync(discountCode.Id, discountCode.Version, actions);
        }

        /// <summary>
        /// Updates a DiscountCode.
        /// </summary>
        /// <param name="discountCodeId">ID of the DiscountCode</param>
        /// <param name="version">The expected version of the discount code on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the discount code.</param>
        /// <returns>DiscountCode</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-discountCodes.html#update-discountcode"/>
        public Task<Response<DiscountCode>> UpdateDiscountCodeAsync(string discountCodeId, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(discountCodeId))
            {
                throw new ArgumentException($"{nameof(discountCodeId)} is required");
            }

            if (version < 1)
            {
                throw new ArgumentException($"{nameof(version)} should to be greater than zero");
            }

            if (actions == null || actions.Count < 1)
            {
                throw new ArgumentException("One or more update actions is required");
            }

            var data = JObject.FromObject(new
            {
                version,
                actions = JArray.FromObject(actions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore })
            });

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", discountCodeId);
            return _client.PostAsync<DiscountCode>(endpoint, data.ToString());
        }
        #endregion
    }
}
