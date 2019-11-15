using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class ChangeCurrenciesUpdateAction : UpdateAction<Project>
    {
        public string Action => "changeCurrencies";
        public List<string> Currencies { get; set; }
    }
}
