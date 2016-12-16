using commercetools.Carts;

using Newtonsoft.Json;

namespace commercetools.Customers
{
    /// <summary>
    /// CustomerSignInResult
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-customers.html#customersigninresult"/>
    public class CustomerSignInResult
    {
        #region Properties

        /// <summary>
        /// Customer
        /// </summary>
        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; private set; }

        /// <summary>
        /// Cart
        /// </summary>
        [JsonProperty(PropertyName = "cart")]
        public Cart Cart { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public CustomerSignInResult(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Customer = new Customer(data.customer);
            this.Cart = new Cart(data.cart);
        }

        #endregion
    }
}
