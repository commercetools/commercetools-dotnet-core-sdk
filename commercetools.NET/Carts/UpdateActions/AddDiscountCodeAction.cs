using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Adds a DiscountCode to the cart to enable the related CartDiscounts.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#add-discountcode"/>
    public class AddDiscountCodeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// The code of an existing DiscountCode.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">The code of an existing DiscountCode.</param>
        public AddDiscountCodeAction(string code)
        {
            this.Action = "addDiscountCode";
            this.Code = code;
        }

        #endregion
    }
}
