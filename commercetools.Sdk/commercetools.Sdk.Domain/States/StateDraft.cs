using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.States
{
    public class StateDraft : IDraft<State>
    {
        public string Key { get; set; }
        public StateType Type { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public bool Initial { get; set; }
        public List<StateRole> Roles { get; set; }
        public List<IReferenceable<State>> Transitions { get; set; }
    }
}
