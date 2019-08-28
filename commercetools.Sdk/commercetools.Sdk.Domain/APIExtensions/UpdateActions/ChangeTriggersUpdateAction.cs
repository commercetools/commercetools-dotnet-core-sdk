using System.Collections.Generic;

namespace commercetools.Sdk.Domain.APIExtensions.UpdateActions
{
    public class ChangeTriggersUpdateAction : UpdateAction<Extension>
    {
        public string Action => "changeTriggers";
        public List<Trigger> Triggers { get; set; }
    }
}
