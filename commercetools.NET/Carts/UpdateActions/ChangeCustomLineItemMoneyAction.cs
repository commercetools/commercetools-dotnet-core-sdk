using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the money of the given CustomLineItem.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#change-customlineitem-money"/>
    public class ChangeCustomLineItemMoneyAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ID of an existing CustomLineItem in the cart.
        /// </summary>
        [JsonProperty(PropertyName = "customLineItemId")]
        public string CustomLineItemId { get; set; }

        /// <summary>
        /// The new money.
        /// </summary>
        [JsonProperty(PropertyName = "money")]
        public Money Money { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="customLineItemId">ID of an existing CustomLineItem in the cart.</param>
        /// <param name="money">The new money.</param>
        public ChangeCustomLineItemMoneyAction(string customLineItemId, Money money)
        {
            this.Action = "changeCustomLineItemMoney";
            this.CustomLineItemId = customLineItemId;
            this.Money = money;
        }

        #endregion
    }
}
