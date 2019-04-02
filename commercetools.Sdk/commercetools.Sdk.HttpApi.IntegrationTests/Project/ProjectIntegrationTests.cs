using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Project;
using commercetools.Sdk.Domain.Project.UpdateActions;
using Xunit;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Project
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
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;
            Assert.True(project.Countries.Count > 0);
            Assert.True(project.Languages.Count > 0);
        }

        #region UpdateActions

        [Fact]
        public void SetShippingRateInputTypeToCartScore()
        {
            IClient commerceToolsClient = this.projectFixture.GetService<IClient>();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;

            Assert.NotNull(project);

            var updatedProject = this.projectFixture.SetShippingRateInputTypeToCartScore();

            Assert.NotNull(updatedProject.ShippingRateInputType);
            Assert.IsType(typeof(CartScoreShippingRateInputType), updatedProject.ShippingRateInputType);

            // then remove it
            updatedProject = this.projectFixture.RemoveExistingShippingRateInputType();

            Assert.Null(updatedProject.ShippingRateInputType);
        }
        [Fact]
        public void SetShippingRateInputTypeToCartClassification()
        {
            IClient commerceToolsClient = this.projectFixture.GetService<IClient>();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;

            Assert.NotNull(project);

            var classificationValues = this.projectFixture
                .GetCartClassificationTestValues();
            var updatedProject =
                this.projectFixture.SetShippingRateInputTypeToCartClassification(classificationValues);

            Assert.NotNull(updatedProject.ShippingRateInputType);
            Assert.IsType(typeof(CartClassificationShippingRateInputType), updatedProject.ShippingRateInputType);

            // then remove it
            updatedProject = this.projectFixture.RemoveExistingShippingRateInputType();

            Assert.Null(updatedProject.ShippingRateInputType);
        }
        #endregion


    }
}
