using commercetools.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Inventory.UpdateActions
{
    /// <summary>
    /// In order to change a Quantity, it sets quantityOnStock and updates availableQuantity according to the new quantity and amount of active reservations.
    /// </summary>
    /// <see href="http://docs.commercetools.com/http-api-projects-inventory.html#change-quantity"/>
    public class ChangeQuantityAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Quantity.
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Number">Number</param>
        public ChangeQuantityAction(int quantity)
        {
            this.Action = "changeQuantity";
            this.Quantity = quantity;
        }

        #endregion
    }
}
