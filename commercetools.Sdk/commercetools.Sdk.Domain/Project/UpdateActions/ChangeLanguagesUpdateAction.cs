using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Project.UpdateActions
{
    public class ChangeLanguagesUpdateAction : UpdateAction<Project>
    {
        public string Action => "changeLanguages";
        public List<string> Languages { get; set; }
    }
}
