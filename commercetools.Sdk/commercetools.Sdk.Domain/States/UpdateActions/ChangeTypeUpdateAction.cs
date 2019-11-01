using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class ChangeTypeUpdateAction : UpdateAction<State>
    {
        public string Action => "changeType";
        public StateType Type { get; set; }
    }
}
