using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

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
        public List<IReference<State>> Transitions { get; set; }
    }
}
