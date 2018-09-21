using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class QueryCommand<T> : Command<PagedQueryResult<T>>
    {
        public QueryCommand(QueryPredicate<T> queryPredicate, Sort<T> sort, Expansion expand, int limit, int offset)
        {

        }
    }
}
