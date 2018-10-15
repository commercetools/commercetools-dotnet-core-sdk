using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    // TODO Add option for the implementors to have a ToString which gives the expanded query
    public class QueryCommand<T> : Command<PagedQueryResult<T>>
    {
        public QueryPredicate<T> QueryPredicate { get; set; }
        public List<Sort<T>> Sort { get; set; }
        public List<Expansion<T>> Expand { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }

        public QueryCommand(QueryPredicate<T> queryPredicate, List<Sort<T>> sort, List<Expansion<T>> expand, int limit, int offset)
        {
            this.QueryPredicate = queryPredicate;
            this.Sort = sort;
            this.Expand = expand;
            this.limit = limit;
            this.offset = offset;
        }
    }
}
