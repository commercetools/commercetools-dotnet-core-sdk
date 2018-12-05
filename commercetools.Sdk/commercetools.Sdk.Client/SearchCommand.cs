namespace commercetools.Sdk.Client
{
    using System.Collections.Generic;
    using Domain;

    public abstract class SearchCommand<T> : Command<PagedQueryResult<T>>
    {
        protected SearchCommand()
        {
            this.Filter = new List<string>();
            this.FilterFacets = new List<string>();
            this.FilterQuery = new List<string>();
            this.Sort = new List<string>();
            this.Facets = new List<string>();
        }

        public TextSearch Text { get; set; }

        public bool? Fuzzy { get; set; }

        public bool? FuzzyLevel { get; set; }

        public List<string> Filter { get; }

        public List<string> FilterQuery { get; }

        public List<string> FilterFacets { get; }

        public List<string> Facets { get; }

        public List<string> Sort { get; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public bool? MarkMatchingVariants { get; set; }

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

        public void SetFilter(List<Filter<T>> filterPredicates)
        {
            if (filterPredicates != null)
            {
                foreach (var filter in filterPredicates)
                {
                    this.Filter.Add(filter.ToString());
                }
            }
        }

        public void SetFilterQuery(List<Filter<T>> filterPredicates)
        {
            if (filterPredicates == null)
            {
                return;
            }

            foreach (var filter in filterPredicates)
            {
                this.FilterQuery.Add(filter.ToString());
            }
        }

        public void SetFilterFacets(List<Filter<T>> filterPredicates)
        {
            if (filterPredicates == null)
            {
                return;
            }

            foreach (var filter in filterPredicates)
            {
                this.FilterFacets.Add(filter.ToString());
            }
        }

        public void SetFacets(List<Facet<T>> facetPredicates)
        {
            if (facetPredicates == null)
            {
                return;
            }

            foreach (var facet in facetPredicates)
            {
                this.Facets.Add(facet.ToString());
            }
        }
    }
}