using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeEnumValueKeyUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeEnumKey";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string NewKey { get; set; }
    }
}