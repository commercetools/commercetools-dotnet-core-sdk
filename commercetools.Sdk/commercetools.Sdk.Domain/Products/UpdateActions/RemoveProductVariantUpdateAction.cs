using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class RemoveProductVariantUpdateAction : UpdateAction<Product>
    {
        public string Action => "removeVariant";
        public int Id { get; set; }
        public string Sku { get; set; }        
        public bool Staged { get; set; }
    }
}