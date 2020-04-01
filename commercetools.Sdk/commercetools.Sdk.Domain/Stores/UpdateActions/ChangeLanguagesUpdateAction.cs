using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Stores.UpdateActions
{
    public class SetLanguagesUpdateAction : UpdateAction<Store>
    {
        public string Action => "setLanguages";
        public List<string> Languages { get; set; }
    }
}
