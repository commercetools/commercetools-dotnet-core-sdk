using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the country of the cart. When the country is set, the LineItem prices are updated.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-country"/>
    public class SetCountryAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// A two-digit country code as per ISO 3166-1 alpha-2.
        /// </summary>
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetCountryAction()
        {
            this.Action = "setCountry";
        }

        #endregion
    }
}
