using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Validation
{
    public interface ICultureValidator
    {
        bool IsCultureValid(string culture);
    }
}
