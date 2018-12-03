using commercetools.Sdk.Domain;
using System.Collections.Generic;

namespace commercetools.Sdk.Client
{
    // TODO Add option for the implementors to have a ToString which gives the expanded query
    public class QueryCommand<T> : Command<PagedQueryResult<T>>
    {
        public override System.Type ResourceType => typeof(T);

        public QueryPredicate<T> QueryPredicate { get; set; }
        public List<Sort<T>> Sort { get; set; }
        public List<Expansion<T>> Expand { get; set; }

        // TODO Implement limit and offset in QB
        public int Limit { get; set; }
        public int Offset { get; set; }

        public QueryCommand()
        {
        }
    }
}