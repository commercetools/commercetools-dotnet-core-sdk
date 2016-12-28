using System.Threading.Tasks;

using commercetools.Common;

namespace commercetools.Project
{
    /// <summary>
    /// Provides access to the functions in the Project section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-project.html"/>    
    public class ProjectManager
    {
        #region Member Variables

        private Client _client;

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="client">Client</param>
        public ProjectManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// ets the current project.
        /// </summary>
        /// <see href="http://dev.commercetools.com/http-api-projects-project.html#get-project"/>
        /// <returns>Project</returns>
        public Task<Response<Project>> GetProjectAsync()
        {
            return _client.GetAsync<Project>(string.Empty);
        }

        #endregion
    }
}
