using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public interface IPredicateQueryable
    {
        List<string> Where { get; set; }
    }
}
