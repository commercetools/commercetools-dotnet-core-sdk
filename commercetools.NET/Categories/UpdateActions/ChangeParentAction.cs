using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Categories.UpdateActions
{
    /// <summary>
    /// Changing parents should not be done concurrently. Concurrent changes of parent categories might currently lead to corrupted ancestor lists (paths).
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#change-parent"/>
    public class ChangeParentAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a category
        /// </summary>
        [JsonProperty(PropertyName = "parent")]
        public Reference Parent { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">Reference to a category</param>
        public ChangeParentAction(Reference parent)
        {
            this.Action = "changeParent";
            this.Parent = parent;
        }

        #endregion
    }
}
