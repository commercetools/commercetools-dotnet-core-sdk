using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Categories.UpdateActions
{
    /// <summary>
    /// ChangeOrderHintAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#change-orderhint"/>
    public class ChangeOrderHintAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Order Hint
        /// </summary>
        [JsonProperty(PropertyName = "orderHint")]
        public string OrderHint { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="orderHint">Order Hint</param>
        public ChangeOrderHintAction(string orderHint)
        {
            this.Action = "changeOrderHint";
            this.OrderHint = orderHint;
        }

        #endregion
    }
}
