using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain
{
    public class ChangeFieldDefinitionOrderUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeFieldDefinitionOrder";
        [Required]
        public List<string> FieldNames { get; set; }
    }
}
