using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Client;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Project
{
    public class ProjectFixture : ClientFixture, IDisposable
    {
        public ProjectFixture(): base()
        {

        }
        public void Dispose()
        {

        }

        /// <summary>
        /// Get Current Project Languages
        /// </summary>
        /// <returns></returns>
        public List<string> GetProjectLanguages()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var command = new GetProjectCommand();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;
            return project.Languages.ToList();
        }
    }
}
