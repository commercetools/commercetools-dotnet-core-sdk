using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// Change Description
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#change-description"/>
    public class ChangeDescriptionAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="description">Description</param>
        public ChangeDescriptionAction(string description)
        {
            this.Action = "changeDescription";
            this.Description = description;
        }

        #endregion
    }
}
