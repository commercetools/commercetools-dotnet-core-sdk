﻿using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Project;
using commercetools.Sdk.Domain.Project.UpdateActions;
using Xunit.Abstractions;
using LocalizedEnumValue = commercetools.Sdk.Domain.Common.LocalizedEnumValue;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Project
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
            var command = new GetProjectCommand();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;
            return project.Languages.ToList();
        }

        public Sdk.Domain.Project.Project SetShippingRateInputTypeToCartScore()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;

            //set ShippingRateInputType to be CartScore
            SetShippingRateInputTypeUpdateAction setShippingRateInputTypeUpdateAction = new SetShippingRateInputTypeUpdateAction
            {
                ShippingRateInputType = new CartScoreShippingRateInputType()
            };
            List<UpdateAction<Sdk.Domain.Project.Project>> updateActions = new List<UpdateAction<Sdk.Domain.Project.Project>> {setShippingRateInputTypeUpdateAction};

            var updatedProject = commerceToolsClient
                .ExecuteAsync(new UpdateProjectCommand(project.Version, updateActions))
                .Result;

            return updatedProject;
        }
        public Sdk.Domain.Project.Project SetShippingRateInputTypeToCartClassification(List<LocalizedEnumValue> values)
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
            List<UpdateAction<Sdk.Domain.Project.Project>> updateActions = new List<UpdateAction<Sdk.Domain.Project.Project>> {setShippingRateInputTypeUpdateAction};

            var updatedProject = commerceToolsClient
                .ExecuteAsync(new UpdateProjectCommand(project.Version, updateActions))
                .Result;

            return updatedProject;
        }
        public Sdk.Domain.Project.Project RemoveExistingShippingRateInputType()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var project = commerceToolsClient.ExecuteAsync(new GetProjectCommand()).Result;

            //set ShippingRateInputType to be CartScore
            SetShippingRateInputTypeUpdateAction setShippingRateInputTypeUpdateAction = new SetShippingRateInputTypeUpdateAction
            {
                ShippingRateInputType = null
            };
            List<UpdateAction<Sdk.Domain.Project.Project>> updateActions = new List<UpdateAction<Sdk.Domain.Project.Project>> {setShippingRateInputTypeUpdateAction};

            var updatedProject = commerceToolsClient
                .ExecuteAsync(new UpdateProjectCommand(project.Version, updateActions))
                .Result;

            return updatedProject;
        }
    }
}
