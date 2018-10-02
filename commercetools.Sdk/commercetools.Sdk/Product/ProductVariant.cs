using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public List<Attribute> Attributes { get; set; }
    }
}