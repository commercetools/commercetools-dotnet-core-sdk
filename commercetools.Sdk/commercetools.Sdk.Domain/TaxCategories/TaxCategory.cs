using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.TaxCategories
{
    [Endpoint("tax-categories")]
    [ResourceType(ReferenceTypeId.TaxCategory)]
    public class TaxCategory : Resource<TaxCategory>, IKeyReferencable<TaxCategory>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TaxRate> Rates { get; set; }
    }
}
