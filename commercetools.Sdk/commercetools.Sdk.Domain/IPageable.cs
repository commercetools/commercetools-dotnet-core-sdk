using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public interface IPageable
    {
        int? Limit { get; set; }
        int? Offset { get; set; }
        bool WithTotal { get; set; }
    }
}
