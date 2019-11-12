using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Projects;
using commercetools.Sdk.Domain.Projects.UpdateActions;
using LocalizedEnumValue = commercetools.Sdk.Domain.Common.LocalizedEnumValue;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Projects
{
    public class ProjectFixture : ClientFixture, IDisposable
    {
        public ProjectFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
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
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;
            return project.Languages.ToList();
        }

        public Project SetShippingRateInputTypeToCartScore()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;

            //set ShippingRateInputType to be CartScore
            SetShippingRateInputTypeUpdateAction setShippingRateInputTypeUpdateAction = new SetShippingRateInputTypeUpdateAction
            {
                ShippingRateInputType = new CartScoreShippingRateInputType()
            };
            List<UpdateAction<Project>> updateActions = new List<UpdateAction<Project>> {setShippingRateInputTypeUpdateAction};

            var updatedProject = commerceToolsClient
                .ExecuteAsync(new UpdateProjectCommand(project.Version, updateActions))
                .Result;

            return updatedProject;
        }
        public Project SetShippingRateInputTypeToCartClassification(List<LocalizedEnumValue> values)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;

            //set ShippingRateInputType to be CartScore
            SetShippingRateInputTypeUpdateAction setShippingRateInputTypeUpdateAction = new SetShippingRateInputTypeUpdateAction
            {
                ShippingRateInputType = new CartClassificationShippingRateInputType
                {
                    Values = values
                }
            };
            List<UpdateAction<Project>> updateActions = new List<UpdateAction<Project>> {setShippingRateInputTypeUpdateAction};

            var updatedProject = commerceToolsClient
                .ExecuteAsync(new UpdateProjectCommand(project.Version, updateActions))
                .Result;

            return updatedProject;
        }
        public Project RemoveExistingShippingRateInputType()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;

            //set ShippingRateInputType to be CartScore
            SetShippingRateInputTypeUpdateAction setShippingRateInputTypeUpdateAction = new SetShippingRateInputTypeUpdateAction
            {
                ShippingRateInputType = null
            };
            List<UpdateAction<Project>> updateActions = new List<UpdateAction<Project>> {setShippingRateInputTypeUpdateAction};

            var updatedProject = commerceToolsClient
                .ExecuteAsync(new UpdateProjectCommand(project.Version, updateActions))
                .Result;

            return updatedProject;
        }

        public Project ChangeProjectLanguages(List<string> languages)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;

            var changeLanguagesUpdateAction = new ChangeLanguagesUpdateAction
            {
                Languages = languages
            };
            var updateActions = new List<UpdateAction<Sdk.Domain.Projects.Project>> {changeLanguagesUpdateAction};

            var updatedProject = commerceToolsClient
                .ExecuteAsync(new UpdateProjectCommand(project.Version, updateActions))
                .Result;

            return updatedProject;
        }
    }
}
