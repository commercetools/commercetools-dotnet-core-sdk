using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class SetRolesUpdateAction : UpdateAction<Channel>
    {
        public string Action => "setRoles";
        public List<ChannelRole> Roles { get; set; }
    }
}
