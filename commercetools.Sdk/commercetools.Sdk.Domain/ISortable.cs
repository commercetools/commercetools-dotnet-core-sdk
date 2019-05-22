using System.Collections.Generic;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Domain
{
    public interface ISortable
    {
        List<string> Sort { get; set; }
    }
}
