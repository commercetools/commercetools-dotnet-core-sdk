using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Client
{
    public abstract class SearchCommand<T> : Command<PagedQueryResult<T>>
    {
        public TextSearch Text;
        public bool Fuzzy { get; set; }
        public bool FuzzyLevel { get; set; }
        public List<string> Filter { get; set; }
        public List<string> FilterQuery { get; set; }
        public List<string> FilterFacets { get; set; }
        public List<Facet<T>> Facets { get; set; }
        public List<string> Sort { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public bool MarkMatchingVariants { get; set; }

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

        public void SetFilter(List<Filter<T>> filterPredicates)
        {
            if (filterPredicates != null)
            {
                this.Filter = new List<string>();
                foreach (var filter in filterPredicates)
                {
                    this.Filter.Add(filter.ToString());
                }
            }
        }

        public void SetFilterQuery(List<Filter<T>> filterPredicates)
        {
            if (filterPredicates != null)
            {
                this.FilterQuery = new List<string>();
                foreach (var filter in filterPredicates)
                {
                    this.FilterQuery.Add(filter.ToString());
                }
            }
        }

        public void SetFilterFacets(List<Filter<T>> filterPredicates)
        {
            if (filterPredicates != null)
            {
                this.FilterFacets = new List<string>();
                foreach (var filter in filterPredicates)
                {
                    this.FilterFacets.Add(filter.ToString());
                }
            }
        }
    }

    public class TextSearch
    {
        // TODO Perhaps add an enum for all language codes
        public string Language { get; set; }

        public string Term { get; set; }
    }
}