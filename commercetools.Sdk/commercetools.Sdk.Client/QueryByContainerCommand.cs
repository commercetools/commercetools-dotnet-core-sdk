using System;
using System.Linq;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    using System.Collections.Generic;
    using Domain;

    public class QueryByContainerCommand<T> : QueryCommand<T>
    {
        public QueryByContainerCommand(string container)
        {
            this.Container = container;
            this.QueryParameters = new QueryCommandParameters();
        }

        public QueryByContainerCommand(string container, IAdditionalParameters<T> additionalParameters)
            : this(container)
        {
            this.AdditionalParameters = additionalParameters;
        }

        public QueryByContainerCommand(string container, IQueryParameters queryParameters)
            : this(container)
        {
            this.QueryParameters = queryParameters;
        }

        public string Container { get; set; }

        public override System.Type ResourceType => typeof(T);

        public IQueryParameters QueryParameters { get; set; }
    }
}