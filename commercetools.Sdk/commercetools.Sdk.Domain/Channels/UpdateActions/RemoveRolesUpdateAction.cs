using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class RemoveRolesUpdateAction : UpdateAction<Channel>
    {
        public string Action => "removeRoles";
        public List<ChannelRole> Roles { get; set; }
    }
}
