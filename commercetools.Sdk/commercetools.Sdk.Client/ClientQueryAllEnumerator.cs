using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class ClientQueryAllEnumerator<TCommand> : IEnumerator<TCommand>
    {
        private const int DefaultOffset = 0;
        private const int DefaultLimit = 20;

        private IClient client;
        private QueryCommand<TCommand> command;

        private List<TCommand> resultSet;
        private int position = -1;

        public ClientQueryAllEnumerator(IClient client, QueryCommand<TCommand> command)
        {
            if (client == null)
            {
                throw new FieldAccessException("Client cannot be null");
            }

            this.client = client;
            this.command = command;
        }

        public bool MoveNext()
        {
            this.position++;
            if (this.command.QueryParameters is IPageable pageableParameters && pageableParameters.Limit == null)
            {
                var limit = DefaultLimit;
                if (this.resultSet == null || this.position % limit == 0)
                {
                    // Retrieve new set of results
                    var originalOffset = pageableParameters.Offset;

                    // Reset offset to original value after retrieveing for an eventual reuse of the command somewhere else
                    pageableParameters.Offset = (originalOffset ?? DefaultOffset) + this.position;
                    RetrieveResults(command);
                    pageableParameters.Offset = originalOffset;
                }

                return this.position % limit < this.resultSet.Count;
            }

            // If command is not paged
            if (this.resultSet == null)
            {
                RetrieveResults(this.command);
            }

            return this.position < this.resultSet.Count;
        }

        private void RetrieveResults(QueryCommand<TCommand> queryCommand)
        {
            var queryTask = this.client.ExecuteAsync(queryCommand);
            this.resultSet = queryTask.Result.Results;
        }

        public void Reset()
        {
            this.resultSet = null;
            this.position = -1;
        }

        public TCommand Current
        {
            get
            {
                if (this.command.QueryParameters is IPageable pageableParameters)
                {
                    var limit = pageableParameters.Limit ?? DefaultLimit;
                    return this.resultSet[this.position % limit];
                }

                return this.resultSet[this.position];
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            this.resultSet?.Clear();

            this.client = null;
            this.command = null;
            this.resultSet = null;
        }
    }
}
