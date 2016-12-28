using commercetools.Common;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Revert all changes, which were made to the staged version of a product and reset to the current version.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#revert-staged-changes"/>
    public class RevertStagedChangesAction : UpdateAction
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public RevertStagedChangesAction()
        {
            this.Action = "revertStagedChanges";
        }

        #endregion
    }
}
