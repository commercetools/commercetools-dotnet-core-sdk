namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetAttributeUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAttribute";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        private SetAttributeUpdateAction(string name, object value, bool staged = true)
        {
            this.Name = name;
            this.Value = value;
            this.Staged = staged;
        }
        public SetAttributeUpdateAction(string sku, string name, object value, bool staged = true) :this(name, value, staged)
        {
            this.Sku = sku;
        }
        public SetAttributeUpdateAction(int variantId, string name, object value, bool staged = true) :this(name, value, staged)
        {
            this.VariantId = variantId;
        }
    }
}
