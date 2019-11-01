using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.States;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.States
{
    public static class StatesFixture
    {
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
    }
}
