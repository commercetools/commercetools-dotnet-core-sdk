using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class ChangeCountriesUpdateAction : UpdateAction<Project>
    {
        public string Action => "changeCountries";
        public List<string> Countries { get; set; }
    }
}
