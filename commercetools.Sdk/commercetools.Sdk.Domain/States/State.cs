using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States
{
    [Endpoint("states")]
    [ResourceType(ReferenceTypeId.State)]
    public class State : Resource<State>, IKeyReferencable<State>
    {
        public string Key { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public bool Initial { get; set; }
        public bool BuiltIn { get; set; }
        public List<StateRole> Roles { get; set; }
        public List<Reference<State>> Transitions { get; set; }


    }
}
