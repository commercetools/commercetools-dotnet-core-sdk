using System;
using commercetools.Sdk.Client;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests.Project
{
    [Collection("Integration Tests")]
    public class ProjectIntegrationTests : IClassFixture<ProjectFixture>
    {
        private readonly ProjectFixture projectFixture;
        
        public ProjectIntegrationTests(ProjectFixture typeFixture)
        {
            this.projectFixture = typeFixture;
        }
        
        [Fact]
        public void GetProject()
        {
            IClient commerceToolsClient = this.projectFixture.GetService<IClient>();
            var command = new GetProjectCommand();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;
            Assert.True(project.Countries.Count > 0);
            Assert.True(project.Languages.Count > 0);
        }

    }
}