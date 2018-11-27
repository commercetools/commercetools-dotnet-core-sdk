using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ChangeLocalizedEnumValueOrderUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeLocalizedEnumValueOrder";
        public string FieldNames { get; set; }
        public List<string> Keys { get; set; }
    }
}