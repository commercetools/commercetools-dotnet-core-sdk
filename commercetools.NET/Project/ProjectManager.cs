using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
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
        public async Task<Project> GetProjectAsync()
        {
            dynamic response = await _client.GetAsync(string.Empty);
            return new Project(response);
        }

        #endregion
    }
}