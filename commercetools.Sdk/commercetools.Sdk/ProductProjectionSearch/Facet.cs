using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public abstract class Facet<T>
    {
        public bool IsCountingProducts { get; set; }
        public string Alias { get; set; }
    }
}
