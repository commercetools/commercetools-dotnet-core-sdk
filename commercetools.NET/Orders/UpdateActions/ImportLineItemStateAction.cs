using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// These import of states does not follow any predefined rules and should be only used if no transitions are defined.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#add-lineitem"/>
    public class ImportLineItemStateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Line item ID
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

        /// <summary>
        /// List of item states
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public List<ItemState> State { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lineItemId">Line item ID</param>
        /// <param name="state">List of item states</param>
        public ImportLineItemStateAction(string lineItemId, List<ItemState> state)
        {
            this.Action = "importLineItemState";
            this.LineItemId = lineItemId;
            this.State = state;
        }

        #endregion
    }
}
