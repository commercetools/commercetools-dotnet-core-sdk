using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Validation
{
    public interface ICountryValidator
    {
        bool IsCountryValid(string country);
    }
}
