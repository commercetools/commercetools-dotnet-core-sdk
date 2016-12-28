using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Removes a discount code from the cart.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#remove-discountcode"/>
    public class RemoveDiscountCodeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a DiscountCode.
        /// </summary>
        [JsonProperty(PropertyName = "discountCode")]
        public Reference DiscountCode { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="discountCode">Reference to a DiscountCode.</param>
        public RemoveDiscountCodeAction(Reference discountCode)
        {
            this.Action = "removeDiscountCode";
            this.DiscountCode = discountCode;
        }

        #endregion
    }
}
