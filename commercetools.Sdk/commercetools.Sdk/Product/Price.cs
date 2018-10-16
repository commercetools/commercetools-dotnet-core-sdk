using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class Price
    {
        public string Id { get; set; }
        public Reference CustomerGroup { get; set; }
        public Money Value { get; set; }
    }
}
