namespace commercetools.Sdk.Domain.Products
{
    using System.ComponentModel.DataAnnotations;

    public class SetAttributeInAllVariantsUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAttributeInAllVariants";
        public bool Staged { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}