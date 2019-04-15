using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.Zones;

namespace commercetools.Sdk.HttpApi.IntegrationTests.States
{
    public class StatesFixture : ClientFixture, IDisposable
    {

        public List<State> StatesToDelete { get; private set; }

        public StatesFixture() : base()
        {
            this.StatesToDelete = new List<State>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.StatesToDelete.Reverse();
            foreach (var state in this.StatesToDelete)
            {
                State deletedState = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<State>(new Guid(state.Id), state.Version)).Result;
            }
        }

    }
}
