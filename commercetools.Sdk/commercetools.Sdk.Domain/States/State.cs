using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.States
{
    [Endpoint("states")]
    [ResourceType(ReferenceTypeId.State)]
    public class State
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Key { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public bool Initial { get; set; }
        public bool BuiltIn { get; set; }
        public List<StateRole> Roles { get; set; }
        public List<Reference<State>> Transitions { get; set; }


    }
}
