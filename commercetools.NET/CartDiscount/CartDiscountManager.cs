using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscount
{
    public class CartDiscountManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/cart-discounts";

        #endregion

        #region Member Variables

        private readonly Client _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public CartDiscountManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a Cart Discount by its ID.
        /// </summary>
        /// <param name="cartDiscountId">CartDiscountId ID</param>
        /// <see href="https://dev.commercetools.com/http-api-projects-cartDiscounts.html#get-cartdiscount-by-id"/>
        /// <returns>CartDiscount</returns>
        public Task<Response<CartDiscount>> GetCartDiscountByIdAsync(string cartDiscountId)
        {
            if (string.IsNullOrWhiteSpace(cartDiscountId))
            {
                throw new ArgumentException("cartdiscountId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", cartDiscountId);
            return _client.GetAsync<CartDiscount>(endpoint);
        }

        /// <summary>
        /// Queries for CartDiscount.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>CartDiscountQueryResult</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-cartDiscounts.html#query-cartdiscounts"/>
        public Task<Response<CartDiscountQueryResult>> QueryCartDiscountsAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<CartDiscountQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new CartDiscount.
        /// </summary>
        /// <param name="cartDiscountDraft">CartDiscountDraft</param>
        /// <returns>CartDiscount</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-cartDiscounts.html#create-a-cartdiscount"/>
        public Task<Response<CartDiscount>> CreateCartDiscountAsync(CartDiscountDraft cartDiscountDraft)
        {
            string payload = JsonConvert.SerializeObject(cartDiscountDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<CartDiscount>(ENDPOINT_PREFIX, payload);
        }
        #endregion
    }
}
