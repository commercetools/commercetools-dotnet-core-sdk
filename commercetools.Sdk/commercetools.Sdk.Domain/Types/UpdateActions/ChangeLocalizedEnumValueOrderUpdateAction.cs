using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class ChangeLocalizedEnumValueOrderUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeLocalizedEnumValueOrder";
        [Required]
        public string FieldName { get; set; }
        [Required]
        public List<string> Keys { get; set; }
    }
}
