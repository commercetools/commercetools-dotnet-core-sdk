using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Client;

namespace commercetools.Sdk.IntegrationTests.Project
{
    public static class ProjectFixture
    {
        /// <summary>
        /// Get Current Project Languages
        /// </summary>
        /// <returns></returns>
        public static List<string> GetProjectLanguages(IClient client)
        {
            var command = new GetProjectCommand();
            var project = client.ExecuteAsync(new GetProjectCommand()).Result;
            return project.Languages.ToList();
        }
    }
}
