using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.States.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.States.StatesFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using ChangeKeyUpdateAction = commercetools.Sdk.Domain.States.UpdateActions.ChangeKeyUpdateAction;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.States.UpdateActions.SetDescriptionUpdateAction;

namespace commercetools.Sdk.IntegrationTests.States
{
    [Collection("Integration Tests")]
    public class StatesIntegrationTests
    {
        private readonly IClient client;

        public StatesIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateState()
        {
            var key = $"CreateState-{TestingUtility.RandomString()}";
            await WithState(
                client, stateDraft => DefaultStateDraftWithKey(stateDraft, key),
                state => { Assert.Equal(key, state.Key); });
        }

        [Fact]
        public async Task GetStateById()
        {
            var key = $"GetStateById-{TestingUtility.RandomString()}";
            await WithState(
                client, stateDraft => DefaultStateDraftWithKey(stateDraft, key),
                async state =>
                {
                    var retrievedState = await client
                        .ExecuteAsync(state.ToIdResourceIdentifier().GetById());
                    Assert.Equal(key, retrievedState.Key);
                });
        }

        [Fact]
        public async Task GetStateByKey()
        {
            var key = $"GetStateByKey-{TestingUtility.RandomString()}";
            await WithState(
                client, stateDraft => DefaultStateDraftWithKey(stateDraft, key),
                async state =>
                {
                    var retrievedState = await client
                        .ExecuteAsync(state.ToKeyResourceIdentifier().GetByKey());
                    Assert.Equal(key, retrievedState.Key);
                });
        }

        [Fact]
        public async Task QueryStates()
        {
            var key = $"QueryStates-{TestingUtility.RandomString()}";
            await WithState(
                client, stateDraft => DefaultStateDraftWithKey(stateDraft, key),
                async state =>
                {
                    var queryCommand = new QueryCommand<State>();
                    queryCommand.Where(p => p.Key == state.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteStateById()
        {
            var key = $"DeleteStateById-{TestingUtility.RandomString()}";
            await WithState(
                client, stateDraft => DefaultStateDraftWithKey(stateDraft, key),
                async state =>
                {
                    await client.ExecuteAsync(state.DeleteById());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<State>(state))
                    );
                });
        }

        [Fact]
        public async Task DeleteStateByKey()
        {
            var key = $"DeleteStateByKey-{TestingUtility.RandomString()}";
            await WithState(
                client, stateDraft => DefaultStateDraftWithKey(stateDraft, key),
                async state =>
                {
                    await client.ExecuteAsync(state.DeleteByKey());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<State>(state))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateStateByIdChangeKey()
        {
            await WithUpdateableState(client, async state =>
            {
                var key = TestingUtility.RandomString();
                var action = new ChangeKeyUpdateAction
                {
                    Key = key
                };

                var updatedState = await client
                    .ExecuteAsync(state.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(key, updatedState.Key);
                return updatedState;
            });
        }

        [Fact]
        public async Task UpdateStateByKeySetName()
        {
            await WithUpdateableState(client, async state =>
            {
                var name = TestingUtility.RandomString();
                var action = new SetNameUpdateAction
                {
                    Name = new LocalizedString {{"en", name},}
                };

                var updatedState = await client
                    .ExecuteAsync(state.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(name, updatedState.Name["en"]);
                return updatedState;
            });
        }

        [Fact]
        public async Task UpdateStateBySetDescription()
        {
            await WithUpdateableState(client, async state =>
            {
                var description = TestingUtility.RandomString();
                var action = new SetDescriptionUpdateAction
                {
                    Description = new LocalizedString
                    {
                        {"en", description}
                    }
                };

                var updatedState = await client
                    .ExecuteAsync(state.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(description, updatedState.Description["en"]);
                return updatedState;
            });
        }

        [Fact]
        public async Task UpdateStateChangeType()
        {
            await WithUpdateableState(client, stateDraft =>
                    DefaultStateDraftWithType(stateDraft, StateType.ProductState),
                async state =>
                {
                    Assert.Equal(StateType.ProductState, state.Type);
                    var newState = StateType.OrderState;
                    var action = new ChangeTypeUpdateAction
                    {
                        Type = newState
                    };

                    var updatedState = await client
                        .ExecuteAsync(state.UpdateByKey(actions => actions.AddUpdate(action)));

                    Assert.Equal(newState, updatedState.Type);
                    return updatedState;
                });
        }

        [Fact]
        public async Task UpdateStateChangeInitial()
        {
            await WithUpdateableState(client, async state =>
            {
                var newInitialFlag = !state.Initial;
                var action = new ChangeInitialUpdateAction
                {
                    Initial = newInitialFlag
                };

                var updatedState = await client
                    .ExecuteAsync(state.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(newInitialFlag, updatedState.Initial);
                return updatedState;
            });
        }

        [Fact]
        public async Task UpdateStateSetRoles()
        {
            await WithUpdateableState(client,
                stateDraft => DefaultStateDraftWithType(stateDraft, StateType.LineItemState),
                async state =>
                {
                    Assert.Empty(state.Roles);
                    var stateRoles = new List<StateRole>
                    {
                        StateRole.Return
                    };
                    var action = new SetRolesUpdateAction
                    {
                        Roles = stateRoles
                    };

                    var updatedState = await client
                        .ExecuteAsync(state.UpdateByKey(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedState.Roles);
                    Assert.Equal(StateRole.Return, updatedState.Roles[0]);
                    return updatedState;
                });
        }

        [Fact]
        public async Task UpdateStateAddRoles()
        {
            await WithUpdateableState(client,
                stateDraft => DefaultStateDraftWithType(stateDraft, StateType.LineItemState),
                async state =>
                {
                    Assert.Empty(state.Roles);
                    var stateRoles = new List<StateRole>
                    {
                        StateRole.Return
                    };
                    var action = new AddRolesUpdateAction
                    {
                        Roles = stateRoles
                    };

                    var updatedState = await client
                        .ExecuteAsync(state.UpdateByKey(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedState.Roles);
                    Assert.Equal(StateRole.Return, updatedState.Roles[0]);
                    return updatedState;
                });
        }

        [Fact]
        public async Task UpdateStateRemoveRoles()
        {
            var oldRoles = new List<StateRole> {StateRole.Return};
            await WithUpdateableState(client,
                stateDraft => DefaultStateDraftWithRoles(stateDraft, oldRoles),
                async state =>
                {
                    Assert.Single(state.Roles);
                    var removedRoles = new List<StateRole>
                    {
                        StateRole.Return
                    };
                    var action = new RemoveRolesUpdateAction
                    {
                        Roles = removedRoles
                    };

                    var updatedState = await client
                        .ExecuteAsync(state.UpdateByKey(actions => actions.AddUpdate(action)));

                    Assert.Empty(updatedState.Roles);
                    return updatedState;
                });
        }

        [Fact]
        public async Task UpdateStateSetTransitions()
        {
            await WithState(client, draft =>
                {
                    var randomInt = TestingUtility.RandomInt();
                    var stateDraft = DefaultStateDraft(draft);
                    stateDraft.Name = new LocalizedString() {{"en", $"Shipped_{randomInt}"}};
                    stateDraft.Initial = false;
                    stateDraft.Type = StateType.OrderState;
                    return stateDraft;
                },
                async shippedState =>
                {
                    await WithUpdateableState(client,
                        initialStateDraft => DefaultStateDraftWithType(initialStateDraft, StateType.OrderState),
                        async initialState =>
                        {
                            Assert.Null(initialState.Transitions);
                            var transitionTo = shippedState.ToKeyResourceIdentifier();
                            var action = new SetTransitionsUpdateAction
                            {
                                Transitions = new List<IReference<State>>
                                {
                                    transitionTo
                                }
                            };

                            var updatedInitialState = await client
                                .ExecuteAsync(initialState.UpdateByKey(actions => actions.AddUpdate(action)));

                            Assert.NotNull(updatedInitialState.Transitions);
                            Assert.Equal(shippedState.Id, updatedInitialState.Transitions[0].Id);
                            return updatedInitialState;
                        });
                });
        }

        #endregion
    }
}