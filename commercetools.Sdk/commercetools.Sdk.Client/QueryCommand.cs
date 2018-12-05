using commercetools.Sdk.Domain;
using System.Collections.Generic;

namespace commercetools.Sdk.Client
{
    public class QueryCommand<T> : Command<PagedQueryResult<T>>
    {
        public override System.Type ResourceType => typeof(T);
        public string Where { get; set; }
        public List<string> Sort { get; set; }
        public List<string> Expand { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }

        public void SetSort(List<Sort<T>> sortPredicates)
        {
            if (sortPredicates != null)
            {
                this.Sort = new List<string>();
                foreach (var sort in sortPredicates)
                {
                    this.Sort.Add(sort.ToString());
                }
            }
        }

        public void SetExpand(List<Expansion<T>> expandPredicates)
        {
            if (expandPredicates != null)
            {
                this.Expand = new List<string>();
                foreach (var expand in expandPredicates)
                {
                    this.Expand.Add(expand.ToString());
                }
            }
        }

        public void SetWhere(QueryPredicate<T> queryPredicate)
        {
            if (queryPredicate != null)
            {
                this.Where = queryPredicate.ToString();
            }
        }
    }
}