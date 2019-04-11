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
            this.Sort = new List<string>();
            this.Expand = new List<string>();
            this.Where = new List<string>();
        }

        public QueryCommand(IAdditionalParameters<T> additionalParameters)
        {
            this.Sort = new List<string>();
            this.Expand = new List<string>();
            this.Where = new List<string>();
            this.AdditionalParameters = additionalParameters;
        }

        public List<string> Expand { get; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public override System.Type ResourceType => typeof(T);

        public List<string> Sort { get; }

        public List<string> Where { get; }

        public void SetExpand(IEnumerable<Expansion<T>> expandPredicates)
        {
            if (expandPredicates == null)
            {
                return;
            }

            this.SetExpand(expandPredicates.Select(predicate => predicate.ToString()));
        }

        public void SetExpand(IEnumerable<string> expandPredicates)
        {
            if (expandPredicates == null)
            {
                return;
            }

            this.Expand.Clear();
            foreach (var expand in expandPredicates)
            {
                this.Expand.Add(expand);
            }
        }

        public void SetSort(IEnumerable<Sort<T>> sortPredicates)
        {
            if (sortPredicates == null)
            {
                return;
            }

            this.SetSort(sortPredicates.Select(predicate => predicate.ToString()));
        }

        public void SetSort(IEnumerable<string> sortPredicates)
        {
            if (sortPredicates == null)
            {
                return;
            }

            this.Sort.Clear();
            foreach (var sort in sortPredicates)
            {
                this.Sort.Add(sort);
            }
        }

        public void SetWhere(IEnumerable<QueryPredicate<T>> queryPredicate)
        {
            if (queryPredicate == null)
            {
                return;
            }

            this.SetWhere(queryPredicate.Select(predicate => predicate.ToString()));
        }

        public void SetWhere(IEnumerable<string> queryPredicate)
        {
            if (queryPredicate == null)
            {
                return;
            }

            this.Where.Clear();
            foreach (var query in queryPredicate)
            {
                this.Where.Add(query);
            }
        }

        public void SetWhere(QueryPredicate<T> queryPredicate)
        {
            if (queryPredicate == null)
            {
                return;
            }

            this.SetWhere(queryPredicate.ToString());
        }

        public void SetWhere(string queryPredicate)
        {
            if (queryPredicate == null)
            {
                return;
            }

            this.Where.Clear();
            this.Where.Add(queryPredicate);
        }
    }
}
