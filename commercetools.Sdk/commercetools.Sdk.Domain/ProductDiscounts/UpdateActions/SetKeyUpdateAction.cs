namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}