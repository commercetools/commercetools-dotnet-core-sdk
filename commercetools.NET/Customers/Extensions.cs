using commercetools.Common;

namespace commercetools.Customers
{
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the CustomerManager.
        /// </summary>
        /// <returns>CustomerManager</returns>
        public static CustomerManager Customers(this Client client)
        {
            return new CustomerManager(client);
        }
    }
}