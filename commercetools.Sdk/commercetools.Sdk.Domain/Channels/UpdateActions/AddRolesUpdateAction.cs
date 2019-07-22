using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class AddRolesUpdateAction : UpdateAction<Channel>
    {
        public string Action => "addRoles";
        public List<ChannelRole> Roles { get; set; }
    }
}
