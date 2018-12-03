using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [Endpoint("tax-categories")]
    public class TaxCategoryDraft : IDraft<TaxCategory>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TaxRate> Rates { get; set; }
    }
}