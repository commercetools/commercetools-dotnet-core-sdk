using commercetools.Sdk.Client;

namespace commercetools.Sdk.HttpApi.CommandBuilders
{
    /// <summary>
    /// the entryPoint to domains extension methods
    /// </summary>
    public class CommandBuilder
    {
        public CommandBuilder()
        {
        }

        public CommandBuilder(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; }
    }
}