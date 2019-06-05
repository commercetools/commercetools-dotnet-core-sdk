using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class SetTransitionsUpdateAction : UpdateAction<State>
    {
        public string Action => "setTransitions";
        public List<IReference<State>> Transitions { get; set; }
    }
}
