using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// Adds a product variant in the given quantity to the cart.
    /// </summary>
    /// <remarks>
    /// If the cart already contains the product variant for the given supply and distribution channel, then only quantity of the LineItem is increased.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#add-lineitem"/>
    public class AddReturnInfoAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ReturnDate
        /// </summary>
        [JsonProperty(PropertyName = "returnDate")]
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// Identifies, which return tracking ID is connected to this particular return.
        /// </summary>
        [JsonProperty(PropertyName = "returnTrackingId")]
        public string ReturnTrackingId { get; set; }

        /// <summary>
        /// The items to be returned.
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public List<ReturnItemDraft> Items { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">The items to be returned</param>
        public AddReturnInfoAction(List<ReturnItemDraft> items)
        {
            this.Action = "addReturnInfo";
            this.Items = items;
        }

        #endregion
    }
}
