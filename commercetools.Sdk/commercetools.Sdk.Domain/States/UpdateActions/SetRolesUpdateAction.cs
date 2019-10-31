using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class SetRolesUpdateAction : UpdateAction<State>
    {
        public string Action => "setRoles";
        public List<StateRole> Roles { get; set; }
    }
}
