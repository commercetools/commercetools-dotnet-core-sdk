namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class RecalculateUpdateAction : CartUpdateAction
    {
        public override string Action => "recalculate";
        public bool UpdateProductData { get; set; }
    }
}