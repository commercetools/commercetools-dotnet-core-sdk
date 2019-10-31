using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class SetNameUpdateAction : UpdateAction<State>
    {
        public string Action => "setName";
        public LocalizedString Name { get; set; }
    }
}
