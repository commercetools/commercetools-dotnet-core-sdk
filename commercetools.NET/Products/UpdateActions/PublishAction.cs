using commercetools.Common;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Publishes a product, which causes the staged projection of the product to override the current projection.
    /// </summary>
    /// <remarks>
    /// If the product is published for the first time, the current projection is created.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#publish"/>
    public class PublishAction : UpdateAction
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public PublishAction()
        {
            this.Action = "publish";
        }

        #endregion
    }
}
