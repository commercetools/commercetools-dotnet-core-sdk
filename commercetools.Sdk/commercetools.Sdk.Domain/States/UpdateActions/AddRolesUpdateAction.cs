using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class AddRolesUpdateAction : UpdateAction<State>
    {
        public string Action => "addRoles";
        public List<StateRole> Roles { get; set; }
    }
}
