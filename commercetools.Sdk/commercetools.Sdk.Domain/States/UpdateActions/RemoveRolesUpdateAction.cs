using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class RemoveRolesUpdateAction : UpdateAction<State>
    {
        public string Action => "removeRoles";
        public List<StateRole> Roles { get; set; }
    }
}
