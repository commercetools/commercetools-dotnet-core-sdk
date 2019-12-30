using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.States.UpdateActions;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.States
{
    public static class StatesFixture
    {
        private const string InitialStateKey = "Initial";//given from the platform
        private const string NextStateKey = "NextState";
        
        #region DraftBuilds

        public static StateDraft DefaultStateDraft(StateDraft stateDraft)
        {
            var randomInt = TestingUtility.RandomInt();
            stateDraft.Initial = true;
            stateDraft.Name = new LocalizedString() {{"en", $"State_Name_{randomInt}"}};
            stateDraft.Description = new LocalizedString() {{"en", $"State_Description_{randomInt}"}};
            stateDraft.Key = $"Key{randomInt}";
            stateDraft.Type = StateType.ProductState;
            return stateDraft;
        }
        public static StateDraft DefaultStateDraftWithKey(StateDraft draft, string key)
        {
            var stateDraft = DefaultStateDraft(draft);
            stateDraft.Key = key;
            return stateDraft;
        }
        public static StateDraft DefaultStateDraftWithRoles(StateDraft draft, List<StateRole> roles)
        {
            var stateDraft = DefaultStateDraftWithType(draft, StateType.LineItemState);
            stateDraft.Roles = roles;
            return stateDraft;
        }
        public static StateDraft DefaultStateDraftWithType(StateDraft draft, StateType stateType)
        {
            var stateDraft = DefaultStateDraft(draft);
            stateDraft.Type = stateType;
            return stateDraft;
        }

        #endregion

        #region WithState

        public static async Task WithState( IClient client, Action<State> func)
        {
            await With(client, new StateDraft(), DefaultStateDraft, func);
        }
        public static async Task WithState( IClient client, Func<StateDraft, StateDraft> draftAction, Action<State> func)
        {
            await With(client, new StateDraft(), draftAction, func);
        }

        public static async Task WithState( IClient client, Func<State, Task> func)
        {
            await WithAsync(client, new StateDraft(), DefaultStateDraft, func);
        }
        public static async Task WithState( IClient client, Func<StateDraft, StateDraft> draftAction, Func<State, Task> func)
        {
            await WithAsync(client, new StateDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableState

        public static async Task WithUpdateableState(IClient client, Func<State, State> func)
        {
            await WithUpdateable(client, new StateDraft(), DefaultStateDraft, func);
        }

        public static async Task WithUpdateableState(IClient client, Func<StateDraft, StateDraft> draftAction, Func<State, State> func)
        {
            await WithUpdateable(client, new StateDraft(), draftAction, func);
        }

        public static async Task WithUpdateableState(IClient client, Func<State, Task<State>> func)
        {
            await WithUpdateableAsync(client, new StateDraft(), DefaultStateDraft, func);
        }
        public static async Task WithUpdateableState(IClient client, Func<StateDraft, StateDraft> draftAction, Func<State, Task<State>> func)
        {
            await WithUpdateableAsync(client, new StateDraft(), draftAction, func);
        }

        #endregion
        
        
        /// <summary>
        /// Provides states where the first one is an initial state and has a transition to the second one.
        /// The states may reused and won't be deleted.
        /// </summary>
        /// <returns></returns>
        public static List<State> GetStandardStates(IClient client)
        {
            var standardStates = new List<State>();
            
            var initialState = CreateOrRetrieveByKey<State, StateDraft>(client, InitialStateKey, new StateDraft(),
                draft =>
                {
                    var stateDraft = DefaultStateDraftWithType(draft, StateType.LineItemState);
                    stateDraft.Key = InitialStateKey;
                    stateDraft.Initial = true;
                    stateDraft.Name = new LocalizedString() {{"en", InitialStateKey}};
                    return stateDraft;
                });
            var nextState =
                CreateOrRetrieveByKey<State, StateDraft>(client, NextStateKey, new StateDraft(),
                    draft =>
                    {
                        var stateDraft = DefaultStateDraftWithType(draft, StateType.LineItemState);
                        stateDraft.Initial = false;
                        stateDraft.Key = NextStateKey;
                        stateDraft.Name = new LocalizedString() {{"en", NextStateKey}};
                        return stateDraft;
                    });

            if (initialState.Transitions == null || initialState.Transitions.Count == 0)
            {
                var transitionTo = new ResourceIdentifier<State> {Key = NextStateKey};
                var setTransitions = new SetTransitionsUpdateAction
                {
                    Transitions = new List<IReference<State>> {transitionTo}
                };
                var updateActions = new List<UpdateAction<State>> {setTransitions};
                initialState = client
                    .ExecuteAsync(new UpdateByIdCommand<State>(initialState.Id, initialState.Version, updateActions))
                    .Result;
            }

            standardStates.Add(initialState);
            standardStates.Add(nextState);

            return standardStates;
        }
    }
}
