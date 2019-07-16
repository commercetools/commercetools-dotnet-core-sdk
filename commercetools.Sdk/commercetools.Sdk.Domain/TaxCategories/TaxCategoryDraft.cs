using System.Collections.Generic;

namespace commercetools.Sdk.Domain.TaxCategories
{
    [Endpoint("tax-categories")]
    public class TaxCategoryDraft : IDraft<TaxCategory>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TaxRateDraft> Rates { get; set; }
    }
}
