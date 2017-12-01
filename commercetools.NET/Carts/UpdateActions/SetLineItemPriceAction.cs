using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the price of a line item and changes the priceMode of the line item to ExternalPrice LineItemPriceMode.
    /// </summary>
    /// <remarks>
    /// If the price mode of the line item is ExternalPrice and no externalPrice is given, the external price is unset and the priceMode is set to Platform.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-lineitem-price"/>
    public class SetLineItemPriceAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ID of the LineItem.
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

        /// <summary>
        /// External Price
        /// </summary>
        [JsonProperty(PropertyName = "externalPrice")]
        public Money ExternalPrice { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lineItemId">Id of an existing line item.</param>
        public SetLineItemPriceAction(string lineItemId)
        {
            this.Action = "setLineItemPrice";
            this.LineItemId = lineItemId;
        }

        #endregion
    }
}
