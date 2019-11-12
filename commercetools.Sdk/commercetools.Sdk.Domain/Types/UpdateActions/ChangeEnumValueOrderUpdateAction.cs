using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class ChangeEnumValueOrderUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeEnumValueOrder";
        [Required]
        public string FieldName { get; set; }
        [Required]
        public List<string> Keys { get; set; }
    }
}
