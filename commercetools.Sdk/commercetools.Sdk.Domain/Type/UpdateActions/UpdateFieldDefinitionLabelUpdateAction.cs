using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class UpdateFieldDefinitionLabelUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeLabel"; 
        public string FieldName { get; set; }
        public LocalizedString Label { get; set; }
    }
}
