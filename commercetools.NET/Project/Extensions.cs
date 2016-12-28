using commercetools.Common;

namespace commercetools.Project
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the ProjectManager.
        /// </summary>
        /// <returns>ProjectManager</returns>
        public static ProjectManager Project(this Client client)
        {
            return new ProjectManager(client);
        }
    }
}
