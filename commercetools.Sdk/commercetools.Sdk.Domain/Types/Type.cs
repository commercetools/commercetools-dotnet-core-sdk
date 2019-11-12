using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Types
{
    [Endpoint("types")]
    [ResourceType(ReferenceTypeId.Type)]
    public class Type : Resource<Type>, IKeyReferencable<Type>
    {
        public string Key { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public List<ResourceTypeId> ResourceTypeIds { get; set; }
        public List<FieldDefinition> FieldDefinitions { get; set; }

        public FieldDefinition GetFieldDefinition(string index)
        {
            return this.FieldDefinitions.FirstOrDefault(f => f.Name == index);
        }
    }
}
