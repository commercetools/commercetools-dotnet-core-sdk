using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Domain
{
    public class QueryCommandParameters : IQueryParameters, ISortable, IExpandable, IPageable, IPredicateQueryable
    {
        public List<string> Sort { get; set; }
        public List<string> Expand { get; set; }

        public List<string> Where { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public bool WithTotal { get; set; }

        public QueryCommandParameters()
        {
            this.Sort = new List<string>();
            this.Expand = new List<string>();
            this.Where = new List<string>();
        }
    }
}
