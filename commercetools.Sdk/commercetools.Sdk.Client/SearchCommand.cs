using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public abstract class SearchCommand<T> : Command<PagedQueryResult<T>>
    {
        public TextSearch Text = new TextSearch();
        public bool Fuzzy { get; set; }
        public bool FuzzyLevel { get; set; }
        public List<Filter<T>> Filter { get; set; }
        public List<Filter<T>> FilterQuery { get; set; }
        public List<Filter<T>> FilterFacets { get; set; }
        public List<Facet<T>> Facets { get; set; }
        public Sort<T> Sort { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public bool Staged { get; set; }
        public bool MarkMatchingVariants { get; set; }
        // TODO Perhaps add an enum for all price currency codes
        public string PriceCurrency { get; set; }
        // TODO See if a check should be added for object validity (next three properties can only be used in conjunction with price currency)
        // Or combine these 4 properties into a single object
        public string PriceCountry { get; set; }
        public Guid PriceCustomerGroup { get; set; }
        public Guid PriceChannel { get; set;  }      
    }

    public class TextSearch
    {
        // TODO Perhaps add an enum for all language codes
        public string Language { get; set;  }
        public string Term { get; set; }
    }
}
