using commercetools.Common;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Unpublishes a product, effectively deleting the current projection of the product, leaving only the staged projection. 
    /// </summary>
    /// <remarks>
    /// Consequently, when a product is unpublished, it will no longer be included in query or search results issued with staged=false, since such results only include current projections.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#unpublish"/>
    public class UnpublishAction : UpdateAction
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnpublishAction()
        {
            this.Action = "unpublish";
        }

        #endregion
    }
}
