using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.HttpApi.CommandBuilders
{
    public class CommandBuilder<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public CommandBuilder(IClient client)
        {
            this.Client = client;
            this.CreateCommandFunc = null;
        }

        public CommandBuilder(IClient client, Func<TCommand> command)
        : this(client)
        {
            this.CreateCommandFunc = command;
        }

        public IClient Client { get; }

        private Func<TCommand> CreateCommandFunc { get; set; }

        private TCommand CommandToExecute { get; set; }

        public virtual TCommand Build()
        {
            this.CommandToExecute = CreateCommandFunc.Invoke();
            return this.CommandToExecute;
        }

        public async Task<TResult> ExecuteAsync()
        {
            this.Build();
            return await this.Client.ExecuteAsync(CommandToExecute);
        }
    }
}