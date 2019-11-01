using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class ChangeInitialUpdateAction : UpdateAction<State>
    {
        public string Action => "changeInitial";
        public bool Initial { get; set; }
    }
}
