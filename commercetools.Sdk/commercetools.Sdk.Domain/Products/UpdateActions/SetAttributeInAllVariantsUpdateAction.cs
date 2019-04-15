namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetAttributeInAllVariantsUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAttributeInAllVariants";
        public bool Staged { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}