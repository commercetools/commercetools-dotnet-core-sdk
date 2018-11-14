using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class HighPrecisionMoney : Money
    {
        public int PreciseAmount { get; set; }
    }
}
