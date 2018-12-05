namespace commercetools.Sdk.Client
{
    using System.Collections.Generic;
    using Domain;

    public class QueryCommand<T> : Command<PagedQueryResult<T>>
    {
        public QueryCommand()
        {
            this.Sort = new List<string>();
            this.Expand = new List<string>();
        }

        public List<string> Expand { get; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public override System.Type ResourceType => typeof(T);

        public List<string> Sort { get; }

        public string Where { get; set; }

        public void SetExpand(List<Expansion<T>> expandPredicates)
        {
            if (expandPredicates == null)
            {
                return;
            }

            foreach (var expand in expandPredicates)
            {
                this.Expand.Add(expand.ToString());
            }
        }

        public void SetSort(List<Sort<T>> sortPredicates)
        {
            if (sortPredicates == null)
            {
                return;
            }

            foreach (var sort in sortPredicates)
            {
                this.Sort.Add(sort.ToString());
            }
        }

        public void SetWhere(QueryPredicate<T> queryPredicate)
        {
            if (queryPredicate == null)
            {
                return;
            }

            this.Where = queryPredicate.ToString();
        }
    }
}