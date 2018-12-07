namespace commercetools.Sdk.Domain.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ValidationException : Exception
    {
        public IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}
