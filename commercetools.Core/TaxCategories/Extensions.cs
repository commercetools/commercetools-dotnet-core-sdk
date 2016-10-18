using commercetools.Common;

namespace commercetools.TaxCategories
{
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the TaxCategoryManager.
        /// </summary>
        /// <returns>TaxCategoryManager</returns>
        public static TaxCategoryManager TaxCategories(this Client client)
        {
            return new TaxCategoryManager(client);
        }
    }
}