using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.CartDiscounts.UpdateActions
{
    /// <summary>
    /// ChangeStackingModeAction
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-cartDiscounts.html#change-stacking-mode"/>
    public class ChangeStackingModeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// StackingMode
        /// </summary>
        [JsonProperty(PropertyName = "stackingMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StackingMode StackingMode { get; set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stackingMode">StackingMode</param>
        public ChangeStackingModeAction(StackingMode stackingMode)
        {
            this.Action = "changeStackingMode";
            this.StackingMode = stackingMode;
        }

        #endregion
    }
}
