using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public abstract class Money
    {
        public string Type { get; set; }
        public string CurrencyCode { get; set; }
        public int CentAmount { get; set; }
        public int FractionDigits { get; set; }
    }
}
