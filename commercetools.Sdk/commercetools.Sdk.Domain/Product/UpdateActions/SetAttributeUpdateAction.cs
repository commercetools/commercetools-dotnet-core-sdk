namespace commercetools.Sdk.Domain.Products
{
    using System.ComponentModel.DataAnnotations;

    public class SetAttributeUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAttribute";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}