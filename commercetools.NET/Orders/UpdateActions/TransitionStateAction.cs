using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// Transition to a new state.
    /// </summary>
    /// <remarks>
    /// If there is no state yet, the new state must be an initial state. If the existing state has transitions set, there must be a direct transition to the new state. If transitions is not set, no validation is performed. These validations can be turned off by setting the force parameter to true.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#transition-state"/>
    public class TransitionStateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a State
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public Reference State { get; set; }

        /// <summary>
        /// Force - Defaults to false
        /// </summary>
        [JsonProperty(PropertyName = "force")]
        public bool Force { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="state">Reference to a State</param>
        public TransitionStateAction(Reference state)
        {
            this.Action = "transitionState";
            this.State = state;
        }

        #endregion
    }
}
