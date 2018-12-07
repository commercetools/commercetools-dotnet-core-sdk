using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Validation
{
    public interface ICurrencyValidator
    {
        bool IsCurrencyValid(string currency);
    }
}
