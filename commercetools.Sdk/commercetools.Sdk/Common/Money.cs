using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    // TODO This should actually be of derived Money types
    public class Money
    {
        public string Type { get; set; }
        public string CurrencyCode { get; set; }
        public int CentAmount { get; set; }
        public int FractionDigits { get; set; }
    }
}
