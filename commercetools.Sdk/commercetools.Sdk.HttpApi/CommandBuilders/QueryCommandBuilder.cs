using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.HttpApi.CommandBuilders
{
    public class QueryCommandBuilder<T> : CommandBuilder<QueryCommand<T>, PagedQueryResult<T>>
    {
        public QueryCommandBuilder(IClient client, Func<IQueryParameters, QueryCommand<T>> command)
        : base(client)
        {
            this.CreateCommandFunc = command;
            this.QueryParameters = new QueryCommandParameters();
        }

        public IQueryParameters QueryParameters { get; private set; }

        private Func<IQueryParameters, QueryCommand<T>> CreateCommandFunc { get; set; }

        public override QueryCommand<T> Build()
        {
            this.CommandToExecute = CreateCommandFunc.Invoke(QueryParameters);
            return this.CommandToExecute;
        }
    }
}