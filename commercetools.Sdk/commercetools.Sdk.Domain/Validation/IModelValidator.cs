using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.Validation
{
    public interface IModelValidator
    {
        IEnumerable<ValidationResult> IsValid(object model);
    }
}
