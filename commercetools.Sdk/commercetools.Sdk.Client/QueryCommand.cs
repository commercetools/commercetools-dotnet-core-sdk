using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    // TODO Add option for the implementors to have a ToString which gives the expanded query
    // TODO Extend object for specialized search of product projections
    public class QueryCommand<T> : Command<PagedQueryResult<T>>
    {
        public QueryPredicate<T> QueryPredicate { get; set; }
        public List<Sort<T>> Sort { get; set; }
        public List<Expansion<T>> Expand { get; set; }
        // TODO Implement limit and offset in QB
        public int limit { get; set; }
        public int offset { get; set; }

        public QueryCommand()
        {
        }
    }
}
