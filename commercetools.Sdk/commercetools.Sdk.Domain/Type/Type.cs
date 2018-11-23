using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class Type
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Key { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public List<ResourceTypeId> ResourceTypeIds { get; set; }
        public List<FieldDefinition> FieldDefinitions { get; set; }
    }
}