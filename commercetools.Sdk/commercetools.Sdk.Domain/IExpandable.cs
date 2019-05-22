using System.Collections.Generic;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Domain
{
    public interface IExpandable
    {
        List<string> Expand { get; set; }
    }
}
