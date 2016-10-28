using commercetools.Common;

namespace commercetools.Categories
{
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the CategoryManager.
        /// </summary>
        /// <returns>CategoryManager</returns>
        public static CategoryManager Categories(this Client client)
        {
            return new CategoryManager(client);
        }
    }
}
