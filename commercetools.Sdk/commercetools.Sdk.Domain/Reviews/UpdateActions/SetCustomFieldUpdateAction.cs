using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Reviews
{
    public class SetCustomFieldUpdateAction : UpdateAction<Review>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}