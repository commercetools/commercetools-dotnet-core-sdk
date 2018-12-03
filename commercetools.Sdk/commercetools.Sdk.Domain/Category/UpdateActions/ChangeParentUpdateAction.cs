using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Categories
{
    public class ChangeParentUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeParent";
        [Required]
        public ResourceIdentifier Slug { get; set; }
    }
}