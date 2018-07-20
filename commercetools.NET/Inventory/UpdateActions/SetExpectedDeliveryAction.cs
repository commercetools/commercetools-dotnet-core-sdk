using commercetools.Common;
using Newtonsoft.Json;
using System;

namespace commercetools.Inventory.UpdateActions
{
    /// <summary>
    /// Set Expected Delivery Date
    /// </summary>   
    /// <see href="http://docs.commercetools.com/http-api-projects-inventory.html#set-expecteddelivery"/>
    public class SetExpectedDeliveryAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Date.
        /// </summary>
        [JsonProperty(PropertyName = "expectedDelivery")]
        public DateTime ExpectedDelivery { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expectedDelivery">Expected delivery date</param>
        public SetExpectedDeliveryAction(DateTime expectedDelivery)
        {
            this.Action = "setExpectedDelivery";
            this.ExpectedDelivery = expectedDelivery;
        }

        #endregion
    }
}
