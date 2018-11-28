using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ProductTypeDraft
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AttributeDefinitionDraft> Attributes { get; set; }

    }
}