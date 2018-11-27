using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ChangeFieldDefinitionOrderUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeFieldDefinitionOrder";
        public List<string> FieldNames { get; set; }
    }
}