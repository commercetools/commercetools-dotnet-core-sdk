using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Stores
{
    [Endpoint("stores")]
    [ResourceType(ReferenceTypeId.Store)]
    public class Store : Resource<Store>, IKeyReferencable<Store>
    {
        public string Key { get; set; }
        public LocalizedString Name { get; set; }
        
        public List<string> Languages { get; set; }
    }
}
