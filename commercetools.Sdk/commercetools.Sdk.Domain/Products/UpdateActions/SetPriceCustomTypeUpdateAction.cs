namespace commercetools.Sdk.Domain.Products
{
    using System.ComponentModel.DataAnnotations;

    public class SetPriceCustomTypeUpdateAction : UpdateAction<Product>
    {
        public string Action => "setProductPriceCustomType";
        public ResourceIdentifier Type { get; set; }
        [Required]
        public string PriceId { get; set; }
        public bool Staged { get; set; }
        public Fields Fields { get; set; }
    }
}