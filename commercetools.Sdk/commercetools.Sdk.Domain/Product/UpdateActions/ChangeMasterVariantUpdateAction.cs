using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class ChangeMasterVariantUpdateAction : UpdateAction<Product>
    {
        public string Action => "changeMasterVariant";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
    }
}