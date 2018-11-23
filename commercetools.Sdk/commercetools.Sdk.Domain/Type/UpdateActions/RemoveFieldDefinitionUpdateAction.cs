using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class RemoveFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "removeFieldDefinition"; 
        public string FieldName { get; set; }
    }
}
