using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetCustomFieldUpdateAction : UpdateAction<Category>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}