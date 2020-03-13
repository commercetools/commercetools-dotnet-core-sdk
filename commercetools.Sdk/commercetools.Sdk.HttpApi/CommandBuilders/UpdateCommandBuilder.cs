using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.HttpApi.CommandBuilders
{
    public class UpdateCommandBuilder<T> : CommandBuilder<UpdateCommand<T>, T>
    {
        public UpdateCommandBuilder(IClient client, Func<List<UpdateAction<T>>, UpdateCommand<T>> command)
            : base(client)
        {
            this.CreateCommandFunc = command;
            this.Actions = new List<UpdateAction<T>>();
        }

        public List<UpdateAction<T>> Actions { get; }

        private Func<List<UpdateAction<T>>, UpdateCommand<T>> CreateCommandFunc { get; set; }

        public override UpdateCommand<T> Build()
        {
            return CreateCommandFunc.Invoke(Actions);
        }
    }
}