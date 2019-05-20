using System;
using System.Linq;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    using System.Collections.Generic;
    using Domain;

    public class QueryCommand<T> : Command<PagedQueryResult<T>>
    {
        public QueryCommand()
        {
           this.QueryParameters = new QueryCommandParameters();
        }

        public QueryCommand(IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.AdditionalParameters = additionalParameters;
        }

        public QueryCommand(IQueryParameters queryParameters)
            : this()
        {
            this.QueryParameters = queryParameters;
        }

        public override System.Type ResourceType => typeof(T);

        public IQueryParameters QueryParameters { get; set; }
    }
}
