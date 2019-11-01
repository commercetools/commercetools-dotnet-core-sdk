using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class ChangeKeyUpdateAction : UpdateAction<State>
    {
        public string Action => "changeKey";
        public string Key { get; set; }
    }
}
