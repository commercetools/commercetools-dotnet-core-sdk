using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.States.UpdateActions;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.Registration;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.States
{
    public class StatesFixture : ClientFixture, IDisposable
    {

        public List<State> StatesToDelete { get; private set; }

        public StatesFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.StatesToDelete = new List<State>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.StatesToDelete = this.StatesToDelete.OrderByDescending(state => state.Initial).ToList();
            foreach (var state in this.StatesToDelete)
            {
                State deletedState = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<State>(new Guid(state.Id), state.Version)).Result;
            }
        }

        /// <summary>
        /// Get State Draft
        /// </summary>
        /// <param name="stateKey">key of the state</param>
        /// <param name="stateType">type of the state</param>
        /// <param name="initial">is this initial state</param>
        /// <returns></returns>
        public StateDraft GetStateDraft(string stateKey,StateType stateType = StateType.ProductState,bool initial = true, IReference<State> transitionTo = null)
        {
            var stateDraft = new StateDraft
            {
                Key = stateKey,
                Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}},
                Description = new LocalizedString() {{"en", TestingUtility.RandomString(20)}},
                Initial = initial,
                Type = stateType
            };
            if (transitionTo != null)
            {
                stateDraft.Transitions = new List<IReference<State>> {transitionTo};
            }
            return stateDraft;
        }

        public State CreateState(string stateKey,StateType stateType = StateType.ProductState,bool initial = true, IReference<State> transitionTo = null)
        {
            return this.CreateState(this.GetStateDraft(stateKey, stateType, initial, transitionTo));
        }

        public State CreateState(StateDraft stateDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            State state = commerceToolsClient.ExecuteAsync(new CreateCommand<State>(stateDraft)).Result;
            return state;
        }

        /// <summary>
        /// Check if state exists by key then return it, else create it
        /// </summary>
        /// <param name="stateKey"></param>
        /// <param name="stateType"></param>
        /// <param name="initial"></param>
        /// <param name="transitionTo"></param>
        /// <returns></returns>
        public State CreateStateIfNotExists(string stateKey,StateType stateType = StateType.ProductState,bool initial = true, IReference<State> transitionTo = null)
        {
            State state = null;
            IClient commerceToolsClient = this.GetService<IClient>();

            //check if state exists by key
            var queryCommand = new QueryCommand<State>();
            queryCommand.Where(s => s.Key == stateKey);
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            if (returnedSet.Results.Count == 1)
            {
                state = returnedSet.Results[0];
            }
            else
            {
                state = this.CreateState(this.GetStateDraft(stateKey, stateType, initial, transitionTo));
            }

            return state;
        }

        /// <summary>
        /// Provides states where the first one is an initial state and has a transition to the second one.
        /// The states may reused and won't be deleted.
        /// </summary>
        /// <returns></returns>
        public List<State> GetStandardStates()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var standardStates = new List<State>();

            string initialStateKey = "Initial";//given from the platform
            string nextStateKey = "NextState";

            var initialState = this.CreateStateIfNotExists(initialStateKey, StateType.LineItemState);
            var nextState = this.CreateStateIfNotExists(nextStateKey, StateType.LineItemState, false);

            if (initialState.Transitions == null || initialState.Transitions.Count == 0)
            {
                var transitionTo = new ResourceIdentifier<State> {Key = nextStateKey};
                var setTransitions = new SetTransitionsUpdateAction
                {
                    Transitions = new List<IReference<State>> {transitionTo}
                };
                var updateActions = new List<UpdateAction<State>> {setTransitions};
                initialState = commerceToolsClient
                    .ExecuteAsync(new UpdateByIdCommand<State>(initialState.Id, initialState.Version, updateActions))
                    .Result;
            }

            standardStates.Add(initialState);
            standardStates.Add(nextState);

            return standardStates;
        }

    }
}
