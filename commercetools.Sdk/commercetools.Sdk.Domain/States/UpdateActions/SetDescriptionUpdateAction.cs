using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.States.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<State>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}
