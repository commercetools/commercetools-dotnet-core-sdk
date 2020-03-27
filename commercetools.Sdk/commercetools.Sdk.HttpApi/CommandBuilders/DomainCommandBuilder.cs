using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.HttpApi.CommandBuilders
{
    /// <summary>
    /// entryPoint for commands/functions that we can do in this entityType
    /// </summary>
    /// <typeparam name="T">type of entity</typeparam>
    public class DomainCommandBuilder<T>
        where T : Resource<T>
    {
        public DomainCommandBuilder(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; }
    }
}