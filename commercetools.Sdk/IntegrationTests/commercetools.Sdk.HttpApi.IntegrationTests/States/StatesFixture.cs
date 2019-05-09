using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.Registration;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.States
{
    public class StatesFixture : ClientFixture, IDisposable
    {

        public List<State> StatesToDelete { get; private set; }

        public StatesFixture(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
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

        /// <summary>
        /// Get State Draft
        /// </summary>
        /// <param name="stateType">type of the state</param>
        /// <param name="initial">is this initial state</param>
        /// <returns></returns>
        public StateDraft GetStateDraft(StateType stateType = StateType.ProductState,bool initial = true)
        {
            int rand = TestingUtility.RandomInt();
            var stateDraft = new StateDraft
            {
                Key = $"Key-{rand}",
                Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}},
                Description = new LocalizedString() {{"en", TestingUtility.RandomString(20)}},
                Initial = initial,
                Type = stateType
            };
            return stateDraft;
        }

        public State CreateState(StateType stateType = StateType.ProductState,bool initial = true)
        {
            return this.CreateState(this.GetStateDraft());
        }

        public State CreateState(StateDraft stateDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            State state = commerceToolsClient.ExecuteAsync(new CreateCommand<State>(stateDraft)).Result;
            return state;
        }

    }
}
