using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.HttpApi.IntegrationTests.States;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.States
{
    [Collection("Integration Tests")]
    public class StatesIntegrationTests : IClassFixture<StatesFixture>
    {
        private readonly StatesFixture statesFixture;

        public StatesIntegrationTests(StatesFixture statesFixture)
        {
            this.statesFixture = statesFixture;
        }

        [Fact]
        public void CreateState()
        {
            IClient commerceToolsClient = this.statesFixture.GetService<IClient>();
            StateDraft stateDraft = this.statesFixture.GetStateDraft();
            State state = commerceToolsClient
                .ExecuteAsync(new CreateCommand<State>(stateDraft)).Result;
            this.statesFixture.StatesToDelete.Add(state);
            Assert.Equal(stateDraft.Key, state.Key);
        }
    }
}
