using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.HttpApi.CommandBuilders
{
    public interface ICommandBuilder<out TCommand, T>
        //where T : Resource<T>
        where TCommand : ICommand<T>
    {
        IClient Client { get; }

        TCommand Build();
    }
}