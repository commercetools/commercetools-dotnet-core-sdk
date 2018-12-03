using commercetools.Sdk.Domain;
using System.Collections.Generic;

namespace commercetools.Sdk.Client
{
    public class QueryCommand<T> : Command<PagedQueryResult<T>>
    {
        public override System.Type ResourceType => typeof(T);
        public QueryPredicate<T> QueryPredicate { get; set; }
        public List<Sort<T>> Sort { get; set; }
        public List<Expansion<T>> Expand { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}